using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.BLL.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IServiceRepository _serviceRepo;
        public ServiceManager(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        public async Task<int> CreateService(ServiceModel serviceModel)
        {
            return await _serviceRepo.CreateService(serviceModel);
        }

        public async Task<bool> DeleteServiceByIdentificationString(string IdentificationString)
        {
            return await _serviceRepo.DeleteServiceByIdentificationString(IdentificationString);
        }

        public async Task<ServiceModel> GetServiceByIdentificationString(string IdentificationString)
        {
            return await _serviceRepo.GetServiceByIdentificationString(IdentificationString);
        }
    }
 }

