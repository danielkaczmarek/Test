using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlAppointmentConfiguration : EntityTypeConfiguration<SqlAppointment>
    {
        public SqlAppointmentConfiguration()
        {
            ToTable("SqlAppointment", "MAScheduleView");

            Property(p => p.Subject).HasMaxLength(100).IsOptional();
            Property(p => p.Body).HasMaxLength(500).IsOptional();
            Property(p => p.Start).HasColumnType("datetime2").IsRequired();
            Property(p => p.End).HasColumnType("datetime2").IsRequired();
            Property(p => p.RecurrencePattern).HasMaxLength(100).IsOptional();
            Ignore(p => p.TimeZone);
        }
    }
}