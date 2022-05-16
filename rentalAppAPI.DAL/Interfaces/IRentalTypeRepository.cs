using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Interfaces
{
    public interface IRentalTypeRepository
    {
        public RentalTypeModel ToRentalTypeModel(RentalType rentalTypeEntity);
        Task<bool> DeleteRentalTypeById(int id);
        Task<bool> UpdateRentalTypeById(int id, string type);
        Task<ICollection<RentalTypeModel>> GetAllRentalTypes();
        Task<int> CreateRentalType(RentalTypeModel rentalTypeModel);
    }
}
