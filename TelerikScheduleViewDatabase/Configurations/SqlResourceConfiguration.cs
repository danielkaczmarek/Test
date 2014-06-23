using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlResourceConfiguration : EntityTypeConfiguration<SqlResource>
    {
        public SqlResourceConfiguration()
        {
            ToTable("SqlResource", "MAScheduleView");

            Property(p => p.ResourceName).HasMaxLength(100).IsOptional();
            Property(p => p.DisplayName).HasMaxLength(100).IsOptional();
            Ignore(p => p.ResourceType);
        }
    }
}