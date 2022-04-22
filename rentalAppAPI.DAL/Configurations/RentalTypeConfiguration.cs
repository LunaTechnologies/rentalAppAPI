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
    public class RentalTypeConfiguration : IEntityTypeConfiguration<RentalType>
    {
        public void Configure(EntityTypeBuilder<RentalType> builder)
        {
            builder.HasKey(x => x.RentalTypeId);

            builder.Property(x => x.Type)
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15);

        }
    }
}
