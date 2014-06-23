using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;

namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlExceptionAppointmentConfiguration : EntityTypeConfiguration<SqlExceptionAppointment>
    {
        public SqlExceptionAppointmentConfiguration()
        {
            ToTable("SqlExceptionAppointment", "MAScheduleView");

            Property(p => p.Subject).HasMaxLength(100).IsOptional();
            Property(p => p.Body).HasMaxLength(500).IsOptional();
            Property(p => p.Start).HasColumnType("datetime2").IsRequired();
            Property(p => p.End).HasColumnType("datetime2").IsRequired();
            Ignore(p => p.TimeZone);
        }
    }
}