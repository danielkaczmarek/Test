using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlResource : IResource
    {
        public SqlResource()
        {
            SqlAppointmentResources = new ObservableCollection<SqlAppointmentResource>();
            SqlExceptionResources = new ObservableCollection<SqlExceptionResource>();
        }

        public int SqlResourceId { get; set; }
        public int? SqlResourceTypeId { get; set; }
        public string DisplayName { get; set; }
        public string ResourceName { get; set; }
        public string ResourceType
        {
            get { return SqlResourceType.Name; }
            set
            {
                if (SqlResourceType.Name != value)
                {
                    SqlResourceType.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual SqlResourceType SqlResourceType { get; set; }
        public virtual ObservableCollection<SqlAppointmentResource> SqlAppointmentResources { get; set; }
        public virtual ObservableCollection<SqlExceptionResource> SqlExceptionResources { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }

        public bool Equals(IResource other)
        {
            return other != null && other.ResourceName == ResourceName && other.ResourceType == ResourceType;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}