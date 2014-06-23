using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlExceptionOccurrenceConfiguration: EntityTypeConfiguration<SqlExceptionOccurrence> 
    {
        public SqlExceptionOccurrenceConfiguration()
        {
            ToTable("SqlExceptionOccurrence", "MAScheduleView");

            Property(p => p.ExceptionDate).HasColumnType("datetime2");

            Ignore(p => p.Appointment);
        }    
    }
}