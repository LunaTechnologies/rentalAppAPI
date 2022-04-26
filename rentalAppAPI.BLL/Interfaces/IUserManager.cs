using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.BLL.Interfaces
{
    public interface IUserManager
    {
        Task<List<UserModel>> GetAllUsers(); 
    }
}
