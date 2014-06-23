using System.Collections.ObjectModel;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace TelerikScheduleViewDatabase.Entities
{
    public class Category : ICategory
    {
        public Category()
        {
            SqlAppointments = new ObservableCollection<SqlAppointment>();
            SqlExceptionAppointments = new ObservableCollection<SqlExceptionAppointment>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DisplayName { get; set; }
        public string CategoryBrushName { get; set; }

        private Brush categoryBrush;
        public Brush CategoryBrush
        {
            get
            {
                if (categoryBrush == null)
                {
                    categoryBrush = SolidColorBrushHelper.FromNameString(CategoryBrushName);
                }

                return categoryBrush;
            }
            set
            {
                CategoryBrushName = (categoryBrush as SolidColorBrush).Color.ToString().Substring(1);
                categoryBrush = value;
            }
        }

        public virtual ObservableCollection<SqlAppointment> SqlAppointments { get; set; }
        public virtual ObservableCollection<SqlExceptionAppointment> SqlExceptionAppointments { get; set; }

        public bool Equals(ICategory other)
        {
            return DisplayName == other.DisplayName && CategoryName == other.CategoryName;
        }
    }
}