using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlResourceTypeConfiguration : EntityTypeConfiguration<SqlResourceType>
    {
        public SqlResourceTypeConfiguration()
        {
            ToTable("SqlResourceType", "MAScheduleView");

            Property(p => p.Name).HasMaxLength(100).IsRequired();
            Property(p => p.DisplayName).HasMaxLength(100).IsOptional();
        }
    }
}