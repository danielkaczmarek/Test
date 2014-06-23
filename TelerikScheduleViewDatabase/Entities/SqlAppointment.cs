using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ScheduleView;
using Telerik.Windows.Controls.ScheduleView.ICalendar;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlAppointment : IAppointment, IExtendedAppointment, IObjectGenerator<IRecurrenceRule>
    {
        public SqlAppointment()
        {
            SqlAppointmentResources = new ObservableCollection<SqlAppointmentResource>();
            SqlExceptionOccurrences = new ObservableCollection<SqlExceptionOccurrence>();
        }

        public event EventHandler RecurrenceRuleChanged;
        private List<SqlExceptionOccurrence> exceptionOccurrences;
        private List<SqlExceptionAppointment> exceptionAppointments;
        private IList resources;
        private IRecurrenceRule recurrenceRule;

        public int SqlAppointmentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDayEvent { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public Importance Importance { get; set; }
        public string RecurrencePattern { get; set; }
        public int? TimeMarkerId { get; set; }
        public int? CategoryId { get; set; }

        public virtual ObservableCollection<SqlAppointmentResource> SqlAppointmentResources { get; set; }
        public virtual ObservableCollection<SqlExceptionOccurrence> SqlExceptionOccurrences { get; set; }

        //keys
        public virtual TimeMarker TimeMarker { get; set; }
        public virtual Category Category { get; set; }

        ITimeMarker IExtendedAppointment.TimeMarker
        {
            get { return TimeMarker as ITimeMarker; }
            set { TimeMarker = value as TimeMarker; }
        }

        ICategory IExtendedAppointment.Category
        {
            get { return Category as ICategory; }
            set { Category = value as Category; }
        }

        public IList Resources
        {
            get
            {
                if (resources == null)
                {
                    resources = SqlAppointmentResources.Select(p=>p.SqlResource).ToList();
                }
                return resources;
            }
        }

        public IRecurrenceRule RecurrenceRule
        {
            get
            {
                if (recurrenceRule == null && ScheduleViewRepository.Context.Entry(this).State == EntityState.Unchanged)
                {
                    recurrenceRule = GetRecurrenceRule(RecurrencePattern);
                }
                return recurrenceRule;
            }

            set
            {
                if (recurrenceRule != value)
                {
                    if (value == null)
                    {
                        RecurrencePattern = null;
                    }
                    recurrenceRule = value;
                    OnPropertyChanged("RecurrenceRule");
                }
            }
        }

        public IAppointment Copy()
        {
            IAppointment appointment = new SqlAppointment();
            appointment.CopyFrom(this);
            return appointment;
        }

        void IEditableObject.BeginEdit()
        {
            if (exceptionOccurrences == null)
            {
                exceptionOccurrences = new List<SqlExceptionOccurrence>();
            }

            if (exceptionAppointments == null)
            {
                exceptionAppointments = new List<SqlExceptionAppointment>();
            }

            exceptionOccurrences.Clear();
            exceptionOccurrences.AddRange(SqlExceptionOccurrences.ToList());

            exceptionAppointments.Clear();
            exceptionAppointments.AddRange(SqlExceptionOccurrences.Select(p => p.Appointment).Where(p => p != null).ToList());
        }

        void IEditableObject.CancelEdit()
        {
            var exceptionOccurencesToRemove = SqlExceptionOccurrences.Except(exceptionOccurrences);
            foreach (var ex in exceptionOccurencesToRemove)
            {
                ScheduleViewRepository.Context.SqlExceptionOccurrence.Remove(ex);
                if (ex.Appointment != null)
                {
                    ScheduleViewRepository.Context.SqlExceptionAppointment.Remove((SqlExceptionAppointment)ex.Appointment);
                    foreach (var resource in (ex.Appointment as SqlExceptionAppointment).SqlExceptionResources)
                    {
                        ScheduleViewRepository.Context.SqlExceptionResource.Remove(resource);
                    }
                }
            }
        }

        void IEditableObject.EndEdit()
        {
            var temp = SqlAppointmentResources.ToList();

            foreach (var item in temp)
            {
                ScheduleViewRepository.Context.SqlAppointmentResource.Remove(item);
            }

            foreach (var sqlResource in Resources.OfType<SqlResource>())
            {
                ScheduleViewRepository.Context.SqlAppointmentResource.Add(new SqlAppointmentResource { SqlAppointment = this, SqlAppointmentResourceId = sqlResource.SqlResourceId });
            }

            var removedExceptionAppointments = this.exceptionAppointments.Except(this.SqlExceptionOccurrences.Select(o => o.Appointment).OfType<SqlExceptionAppointment>());
            foreach (var exceptionAppointment in removedExceptionAppointments)
            {
                var excResources = exceptionAppointment.SqlExceptionResources.ToList();
                foreach (var item in excResources)
                {
                    ScheduleViewRepository.Context.SqlExceptionResource.Remove(item);
                }
            }
        }

        public bool Equals(IAppointment other)
        {
            var otherAppointment = other as SqlAppointment;
            return otherAppointment != null &&
                   other.Start == Start &&
                   other.End == End &&
                   other.Subject == Subject &&
                   CategoryId == otherAppointment.CategoryId &&
                   TimeMarker == otherAppointment.TimeMarker &&
                   TimeZone == otherAppointment.TimeZone &&
                   IsAllDayEvent == other.IsAllDayEvent &&
                   RecurrenceRule == other.RecurrenceRule;
        }

        public IRecurrenceRule CreateNew()
        {
            return CreateDefaultRecurrenceRule();
        }
        public IRecurrenceRule CreateNew(IRecurrenceRule item)
        {
            var sqlRecurrenceRule = CreateNew();
            sqlRecurrenceRule.CopyFrom(item);
            return sqlRecurrenceRule;
        }

        public IAppointment ShallowCopy()
        {
            var appointment = new SqlExceptionAppointment();
            appointment.CopyFrom(this);
            return appointment;
        }

        void ICopyable<IAppointment>.CopyFrom(IAppointment other)
        {
            IsAllDayEvent = other.IsAllDayEvent;
            Start = other.Start;
            End = other.End;
            Subject = other.Subject;

            var otherAppointment = other as SqlAppointment;
            if (otherAppointment == null)
                return;

            CategoryId = otherAppointment.CategoryId;
            TimeMarker = otherAppointment.TimeMarker;
            RecurrenceRule = other.RecurrenceRule == null ? null : other.RecurrenceRule.Copy() as SqlRecurrenceRule;
            RecurrencePattern = otherAppointment.RecurrencePattern;

            Resources.Clear();
            Resources.AddRange(otherAppointment.Resources);

            Body = otherAppointment.Body;
        }

        private IRecurrenceRule GetRecurrenceRule(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return null;
            }

            var recurrenceRuleGenerator = this as IObjectGenerator<IRecurrenceRule>;
            var recurrenceRule = recurrenceRuleGenerator != null ? recurrenceRuleGenerator.CreateNew() : CreateDefaultRecurrenceRule();
            var recurrencePattern = new RecurrencePattern();
            RecurrencePatternHelper.TryParseRecurrencePattern(pattern, out recurrencePattern);
            recurrenceRule.Pattern = recurrencePattern;
            foreach (SqlExceptionOccurrence exception in SqlExceptionOccurrences)
            {
                recurrenceRule.Exceptions.Add(exception);
            }

            return recurrenceRule;
        }

        private IRecurrenceRule CreateDefaultRecurrenceRule()
        {
            return new SqlRecurrenceRule(this);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}