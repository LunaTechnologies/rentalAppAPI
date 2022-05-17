using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Interfaces
{
    public interface IServiceRepository
    {
        Task<ServiceModel> ToServiceModel(Service serviceEntity);
        Task<bool> DeleteServiceByIdentificationString(string IdentificationString);
        Task<ServiceModel> GetServiceByIdentificationString(string IdentificationString);
        Task<string> CreateService(ServiceModelCreate serviceModel);
        public string RandomString();
    }
}
