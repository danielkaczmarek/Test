using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TelerikScheduleViewDatabase.Entities;
 
namespace TelerikScheduleViewDatabase.Configurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Category","MAScheduleView");

            Property(p => p.CategoryName).HasMaxLength(100).IsOptional();
            Property(p => p.DisplayName).HasMaxLength(100).IsOptional();
            Property(p => p.CategoryBrushName).HasMaxLength(100).IsOptional();

            Ignore(p => p.CategoryBrush);
        }
    }
}