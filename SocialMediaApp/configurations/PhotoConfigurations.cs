using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.Entities;

namespace SocialMediaApp.configurations
{
    public class PhotoConfigurations : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasOne(p=>p.AppUser).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
