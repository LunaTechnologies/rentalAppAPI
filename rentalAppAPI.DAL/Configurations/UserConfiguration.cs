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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.LastLogin)
                .HasColumnType("datetime2");
            builder.Property(x => x.ProfilePicturePath)
                .HasColumnType("nvarchar(150)")
                .HasMaxLength(150);
            builder.Property(x => x.UserType)
                .HasColumnType("nvarchar(15")
                .HasMaxLength(15);
            builder.Property(x => x.Verified)
                .HasColumnType("bit");
        }
    }
}
