using System.Data.Entity;
using System.Linq;
using TelerikScheduleViewDatabase.Configurations;
using TelerikScheduleViewDatabase.Entities;

namespace TelerikScheduleViewDatabase
{
    public class ContextMaster : DbContext
    {
        public ContextMaster() : base("Test2Database") { }

        public DbSet<Category> Category { get; set; }
        public DbSet<SqlAppointment> SqlAppointment { get; set; }
        public DbSet<SqlAppointmentResource> SqlAppointmentResource { get; set; }
        public DbSet<SqlExceptionAppointment> SqlExceptionAppointment { get; set; }
        public DbSet<SqlExceptionOccurrence> SqlExceptionOccurrence { get; set; }
        public DbSet<SqlExceptionResource> SqlExceptionResource { get; set; }
        public DbSet<SqlResource> SqlResources { get; set; }
        public DbSet<SqlResourceType> SqlResourceType { get; set; }
        public DbSet<TimeMarker> TimeMarker { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new SqlAppointmentConfiguration());
            modelBuilder.Configurations.Add(new SqlAppointmentResourceConfiguration());
            modelBuilder.Configurations.Add(new SqlExceptionAppointmentConfiguration());
            modelBuilder.Configurations.Add(new SqlExceptionOccurrenceConfiguration());
            modelBuilder.Configurations.Add(new SqlExceptionResourceConfiguration());
            modelBuilder.Configurations.Add(new SqlResourceConfiguration());
            modelBuilder.Configurations.Add(new SqlResourceTypeConfiguration());
            modelBuilder.Configurations.Add(new TimeMarkerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}