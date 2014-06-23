using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Windows.Controls;

namespace TelerikScheduleViewDatabase.Entities
{
    public class SqlResourceType : IResourceType
    {
        public SqlResourceType()
        {
            SqlResources = new ObservableCollection<SqlResource>();
        }

        public int SqlResourceTypeId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool AllowMultipleSelection { get; set; }

        public virtual ObservableCollection<SqlResource> SqlResources { get; set; }

        public IList Resources
        {
            get { return SqlResources.ToList(); }
        }
    }
}