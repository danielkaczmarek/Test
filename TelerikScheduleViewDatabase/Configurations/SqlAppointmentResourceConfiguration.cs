using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlAppointmentResourceConfiguration : EntityTypeConfiguration<SqlAppointmentResource>
    {
        public SqlAppointmentResourceConfiguration()
        {
            ToTable("SqlAppointmentResource", "MAScheduleView");
        }
    }
}