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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.ServiceId);

            builder.Property(x => x.Title)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);
            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(1500)")
                .HasMaxLength(1500);
            builder.Property(x => x.Price)
                .HasColumnType("numeric");
            builder.Property(x => x.PhoneNumber)
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15);
            builder.Property(x => x.IdentificationString)
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15);
            builder.HasOne(u => u.User)
                .WithMany(s => s.Services)
                .HasForeignKey(f => f.UserId);

            builder.HasOne(x => x.RentalType)
                .WithMany(x => x.Services)
                .HasForeignKey(f => f.RentalTypeId);
        }

    }
}
