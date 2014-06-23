using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace TelerikScheduleViewDatabase.Entities
{
    public class TimeMarker : ITimeMarker
    {
        public TimeMarker()
        {
            SqlAppointments = new ObservableCollection<SqlAppointment>();
            SqlExceptionAppointments = new ObservableCollection<SqlExceptionAppointment>();
        }

        public int TimeMarkerId { get; set; }
        public string TimeMarkerName { get; set; }
        public string TimeMarkerBrushName { get; set; }

        private Brush timeMarkerBrush;
        public Brush TimeMarkerBrush
        {
            get
            {
                if (timeMarkerBrush == null)
                {
                    timeMarkerBrush = SolidColorBrushHelper.FromNameString(TimeMarkerBrushName);
                }

                return timeMarkerBrush;
            }
            set
            {
                TimeMarkerBrushName = (timeMarkerBrush as SolidColorBrush).Color.ToString().Substring(1);
                timeMarkerBrush = value;
            }
        }

        public virtual ObservableCollection<SqlAppointment> SqlAppointments { get; set; }
        public virtual ObservableCollection<SqlExceptionAppointment> SqlExceptionAppointments { get; set; }

        public bool Equals(ITimeMarker other)
        {
            return TimeMarkerName != other.TimeMarkerName;
        }
    }
}