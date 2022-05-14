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
    public class ServiceRepository : IServiceRepository
    {
        public readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceModel> ToServiceModel(Service serviceEntity)
        {
            var userEntity = await _context.Users.Where(x => x.Id == serviceEntity.UserId).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.RentalTypeId == serviceEntity.RentalTypeId).FirstOrDefaultAsync();
            var serviceModel = new ServiceModel
            {
                Title = serviceEntity.Title,
                Description = serviceEntity.Description,
                PhoneNumber = serviceEntity.PhoneNumber,
                Price = serviceEntity.Price,
                Username = userEntity.UserName,
                ServType = rentalEntity.Type,
                IdentificationString = serviceEntity.IdentificationString
            };
            return serviceModel;
        }

        public async Task<bool> DeleteServiceByIdentificationString(string IdentificationString)
        {
            var serviceEntity = await _context.Services.Where(x => x.IdentificationString == IdentificationString).FirstOrDefaultAsync();
            if (serviceEntity == null)
            {
                return false; // nu exista acest serviciu
            }
            _context.Remove(serviceEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceModel> GetServiceByIdentificationString(string IdentificationString)
        {
            var service = await _context.Services.Where(a => a.IdentificationString.Equals(IdentificationString)).FirstOrDefaultAsync();
            if (service != null)
            {
                var serviceReturn = await ToServiceModel(service);
                return serviceReturn;
            }
            else
            {
                var serviceReturn = new ServiceModel();
                return serviceReturn;
            }
        }

        public async Task<int> CreateService(ServiceModel serviceModel)
        {
            int val = 1;
            var userEntity = await _context.Users.Where(x => x.UserName == serviceModel.Username).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.Type == serviceModel.ServType).FirstOrDefaultAsync();
            try
            {
                var service = new Service
                {
                    Title = serviceModel.Title,
                    Description = serviceModel.Description,
                    PhoneNumber = serviceModel.PhoneNumber,
                    Price = serviceModel.Price,
                    UserId = userEntity.Id,
                    RentalTypeId = rentalEntity.RentalTypeId,
                    IdentificationString = serviceModel.IdentificationString
                };
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                return 1;
            }
            finally { val = 0; };
            if (val == 0)
            {
                return 0;
            }
        }
    }
}
