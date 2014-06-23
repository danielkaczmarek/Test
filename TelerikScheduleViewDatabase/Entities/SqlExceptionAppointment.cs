using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ScheduleView;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlExceptionAppointment : IEditableObject, IAppointment, IExtendedAppointment, IObjectGenerator<IRecurrenceRule>
    {
        public SqlExceptionAppointment()
        {
            SqlExceptionOccurrences = new ObservableCollection<SqlExceptionOccurrence>();
            SqlExceptionResources = new ObservableCollection<SqlExceptionResource>();
        }

        public event EventHandler RecurrenceRuleChanged;

        public int SqlExceptionAppointmentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDayEvent { get; set; }
        public int? CategoryId { get; set; }
        public int? TimeMarkerId { get; set; }
        public TimeZoneInfo TimeZone { get; set; }

        private IRecurrenceRule recurrenceRule;
        public IRecurrenceRule RecurrenceRule
        {
            get { return recurrenceRule; }
            set
            {
                if (recurrenceRule != value)
                {
                    recurrenceRule = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList resource;
        public IList Resources
        {
            get
            {
                if (resource == null)
                {
                    resource = new ObservableCollection<SqlResource>();
                    var resources = ScheduleViewRepository.Context.SqlExceptionResource.Where(x => x.SqlExceptionAppointmentId == this.SqlExceptionAppointmentId).Select(x => x.SqlResource);
                    this.resource.AddRange(resources);
                    ((INotifyCollectionChanged)resource).CollectionChanged += resource_CollectionChanged;
                }
                return resource;
            }
        }

        public virtual TimeMarker TimeMarker { get; set; }
        public virtual Category Category { get; set; }
        public virtual ObservableCollection<SqlExceptionOccurrence> SqlExceptionOccurrences { get; set; }
        public virtual ObservableCollection<SqlExceptionResource> SqlExceptionResources { get; set; }

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

        public Importance Importance { get; set; }

        public IAppointment Copy()
        {
            throw new System.NotImplementedException();
        }

        public void BeginEdit()
        {
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
        }

        public bool Equals(IAppointment other)
        {
            var otherAppointment = other as SqlExceptionAppointment;
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
            throw new System.NotImplementedException();
        }

        public IRecurrenceRule CreateNew(IRecurrenceRule item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyFrom(IAppointment other)
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

            Resources.Clear();
            Resources.AddRange(otherAppointment.Resources);

            Body = otherAppointment.Body;
        }

        void resource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var resource in e.NewItems.OfType<SqlResource>())
                    {
                        ScheduleViewRepository.Context.SqlExceptionResource.Add(
                        new SqlExceptionResource
                        {
                            SqlExceptionAppointmentId = this.SqlExceptionAppointmentId,
                            SqlResourceId = resource.SqlResourceId
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var sqlres in e.OldItems)
                    {
                        var itemsToRemove = ScheduleViewRepository.Context.SqlExceptionResource.Where(x => x.SqlResourceId == ((SqlResource)sqlres).SqlResourceId && x.SqlExceptionAppointmentId == this.SqlExceptionAppointmentId).ToList();
                        foreach (var item in itemsToRemove)
                        {
                        	ScheduleViewRepository.Context.SqlExceptionResource.Remove(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}