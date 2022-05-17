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
        public static Random random = new Random();

        public readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceModel> ToServiceModel(Service serviceEntity)
        {
            var userEntity = await _context.Users.Where(x => x.Id == serviceEntity.UserId).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.RentalTypeId == serviceEntity.RentalTypeId).FirstOrDefaultAsync();
            var picturesRepository = new PictureRepository();
            ICollection<Picture> pictures = new List<Picture>();
            pictures = await _context.Pictures.Where(x => x.IdService == serviceEntity.ServiceId).ToListAsync();
            var serviceModel = new ServiceModel
            {
                Title = serviceEntity.Title,
                Description = serviceEntity.Description,
                PhoneNumber = serviceEntity.PhoneNumber,
                Price = serviceEntity.Price,
                Username = userEntity.UserName,
                ServType = rentalEntity.Type,
                IdentificationString = serviceEntity.IdentificationString,
                Pictures = picturesRepository.ToPictureModelList(pictures)
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

        public async Task<string> CreateService(ServiceModelCreate serviceModel)
        {
            var userEntity = await _context.Users.Where(x => x.UserName == serviceModel.Username).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.Type == serviceModel.ServType).FirstOrDefaultAsync();
            var picturesRepository = new PictureRepository();
            if ((userEntity == null) || (rentalEntity == null))
            {
                return "0";
            }
            if ((userEntity != null) && (rentalEntity != null))
            {
                string identificationString = RandomString();
                var service = new Service
                {
                    Title = serviceModel.Title,
                    Description = serviceModel.Description,
                    PhoneNumber = serviceModel.PhoneNumber,
                    Price = serviceModel.Price,
                    UserId = userEntity.Id,
                    RentalTypeId = rentalEntity.RentalTypeId,
                    IdentificationString = identificationString,
                    Pictures = picturesRepository.ToEntityList(serviceModel.Pictures)
                };
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                return identificationString;
            }

            return "0";
        }

        public string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 15)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
