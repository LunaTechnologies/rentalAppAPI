using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public DateTime LastLogin { get; set; }

        public string ProfilePicturePath { get; set; }

        public string UserType { get; set; }

        public bool Verified { get; set; }

        public string RefreshToken { get; set; }

        public virtual ICollection<Service> Services { get; set; }

    }
}
