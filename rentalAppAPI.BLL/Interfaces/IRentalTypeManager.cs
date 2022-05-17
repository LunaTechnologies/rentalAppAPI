using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.BLL.Interfaces
{
    public interface IRentalTypeManager
    {
        Task<bool> DeleteRentalTypeById(int id);
        Task<ICollection<RentalTypeModel>> GetAllRentalTypes();
        Task<int> CreateRentalType(RentalTypeModel rentalTypeModel);
        Task<bool> UpdateRentalTypeById(int id, string type);

    }
}
