using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfilePicturePath { get; set; }
        public string UserType { get; set; }
        public bool Verified { get; set; }
    }
}
