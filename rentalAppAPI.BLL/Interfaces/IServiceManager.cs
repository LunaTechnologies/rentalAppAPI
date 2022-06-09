using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace rentalAppAPI.BLL.Interfaces
{
    public interface IServiceManager
    {
        Task<bool> DeleteServiceByIdentificationString(string IdentificationString);
        Task<ServiceModel> GetServiceByIdentificationString(string IdentificationString);
        Task<string> CreateService(ICollection<IFormFile> pictures, ServiceModelCreate serviceModel, string userName);
        Task<List<ThumbnailServiceModel>> SearchServices(string serviceTitle);
    }
}
