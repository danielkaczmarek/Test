using System.Data.Entity.ModelConfiguration;
using TelerikScheduleViewDatabase.Entities;

namespace TelerikScheduleViewDatabase.Configurations
{
    public class SqlExceptionResourceConfiguration : EntityTypeConfiguration<SqlExceptionResource>
    {
        public SqlExceptionResourceConfiguration()
        {
            ToTable("SqlExceptionResource", "MAScheduleView");
        }
    }
}