using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.BLL.Managers
{
    public class RentalTypeManager : IRentalTypeManager
    {
        private readonly IRentalTypeRepository _rentalTypeRepo;
        public RentalTypeManager(IRentalTypeRepository rentalTypeRepo)
        {
            _rentalTypeRepo = rentalTypeRepo;
        }
        public async Task<int> CreateRentalType(RentalTypeModel rentalTypeModel)
        {
            return await _rentalTypeRepo.CreateRentalType(rentalTypeModel);
        }

        public async Task<bool> DeleteRentalTypeById(int id)
        {
            return await _rentalTypeRepo.DeleteRentalTypeById(id);
        }

        public async Task<ICollection<RentalTypeModel>> GetAllRentalTypes()
        {
            return await _rentalTypeRepo.GetAllRentalTypes();
        }

        public async Task<bool> UpdateRentalTypeById(int id, string type)
        {
            return await _rentalTypeRepo.UpdateRentalTypeById(id,type);
        }
    }
}
