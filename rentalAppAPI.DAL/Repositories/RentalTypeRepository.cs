using Microsoft.EntityFrameworkCore;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Repositories
{
    public class RentalTypeRepository : IRentalTypeRepository
    {
        public readonly AppDbContext _context;
        public RentalTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateRentalType(RentalTypeModel rentalTypeModel)
        {
            var Type = await _context.RentalTypes.Where(x => x.Type == rentalTypeModel.Type).FirstOrDefaultAsync();
            if (Type == null)
            {
                var rentalType = new RentalType
                {
                    Type = rentalTypeModel.Type,
                };
                _context.RentalTypes.Add(rentalType);
                await _context.SaveChangesAsync();
                return 1;
            }
            else return 0;

        }

        public async Task<bool> UpdateRentalTypeById(int id, string type)
        {
            var Type = await _context.RentalTypes.Where(x => x.RentalTypeId == id).FirstOrDefaultAsync();
            if (Type == null)
            {
                return false;
            }
            Type.Type = type;
            _context.RentalTypes.Update(Type);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRentalTypeById(int id)
        {
            var rentalTypeEntity = await _context.RentalTypes.Where(x => x.RentalTypeId == id).FirstOrDefaultAsync();
            if (rentalTypeEntity == null)
            {
                return false;
            }
            _context.Remove(rentalTypeEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<RentalTypeModel>> GetAllRentalTypes()
        {
            ICollection<RentalTypeModel> rentalTypesModel = new List<RentalTypeModel>();
            ICollection<RentalType> rentalTypes = new List<RentalType>(await _context.RentalTypes.ToListAsync());
            foreach (RentalType rentalType in rentalTypes)
            {
                rentalTypesModel.Add(ToRentalTypeModel(rentalType));
            }
            return rentalTypesModel;
        }

        public RentalTypeModel ToRentalTypeModel(RentalType rentalTypeEntity)
        {
            var rentalTypeModel = new RentalTypeModel
            {
                Type = rentalTypeEntity.Type,
            };
            return rentalTypeModel;
        }
    }
}
