using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class TimeMarkerConfiguration : EntityTypeConfiguration<TimeMarker>
    {
        public TimeMarkerConfiguration()
        {
            ToTable("TimeMarker", "MAScheduleView");

            Property(p => p.TimeMarkerName).HasMaxLength(50).IsOptional();
            Property(p => p.TimeMarkerBrushName).HasMaxLength(50).IsOptional();

            Ignore(p => p.TimeMarkerBrush);
        }
    }
}