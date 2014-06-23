using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ScheduleView;
using Telerik.Windows.Controls.ScheduleView.ICalendar;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlRecurrenceRule : ViewModelBase, IRecurrenceRule
    {
        public SqlRecurrenceRule()
        {
            Exceptions = new ObservableCollection<IExceptionOccurrence>();
        }

        public SqlRecurrenceRule(SqlAppointment appointment)
            : this()
        {
            MasterAppointment = appointment;
        }

        public SqlAppointment MasterAppointment { get; private set; }

        private RecurrencePattern pattern;
        public RecurrencePattern Pattern
        {
            get { return pattern; }
            set
            {
                if (pattern != value)
                {
                    pattern = value;
                    MasterAppointment.RecurrencePattern = RecurrencePatternHelper.RecurrencePatternToString(pattern);
                    OnPropertyChanged("Pattern");
                }
            }
        }

        public ICollection<IExceptionOccurrence> Exceptions { get; private set; }

        public IRecurrenceRule Copy()
        {
            var rule = new SqlRecurrenceRule();
            rule.CopyFrom(this);
            return rule;
        }

        public IExceptionOccurrence CreateNew()
        {
            var excOcc = new SqlExceptionOccurrence();
            excOcc.SqlAppointment = MasterAppointment;
            ScheduleViewRepository.Context.SqlExceptionOccurrence.Add(excOcc);
            return excOcc;
        }

        public IExceptionOccurrence CreateNew(IExceptionOccurrence item)
        {
            var sqlExceptionOccurrence = CreateNew();
            sqlExceptionOccurrence.CopyFrom(item);
            return sqlExceptionOccurrence;
        }

        public void CopyFrom(IRecurrenceRule other)
        {
            if (GetType().FullName != other.GetType().FullName)
            {
                throw new ArgumentException("Invalid type");
            }

            if (other is SqlRecurrenceRule)
            {
                MasterAppointment = (other as SqlRecurrenceRule).MasterAppointment;
            }

            Pattern = other.Pattern.Copy();
            Exceptions.Clear();
            foreach (var exception in other.Exceptions)
            {
                Exceptions.Add(exception.Copy() as SqlExceptionOccurrence);
            }
        }

        public IAppointment CreateExceptionAppointment(IAppointment master)
        {
            return (master as SqlAppointment).ShallowCopy();
        }
    }
}