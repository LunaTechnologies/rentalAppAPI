using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentalAppAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(x => x.IdPicture);

            builder.Property(x => x.Path)
                .HasColumnType("nvarchar(150)")
                .HasMaxLength(150);

            builder.HasOne(s => s.Service)
                .WithMany(p =>p.Pictures)
                .HasForeignKey(f => f.IdService);
        }
    }
}
