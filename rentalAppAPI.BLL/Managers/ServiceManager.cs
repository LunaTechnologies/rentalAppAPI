using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using rentalAppAPI.BLL.Models;
using SixLabors.ImageSharp;

namespace rentalAppAPI.BLL.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IImageManager _imageManager;
        public ServiceManager(IServiceRepository serviceRepo, IImageManager imageManager)
        {
            _serviceRepo = serviceRepo;
            _imageManager = imageManager;
        }

        public async Task<string> CreateService(ICollection<IFormFile> pictures, ServiceModelCreate serviceModel, string userName)
        {
            Boolean error = false;
            string errorMessage = "";
            if (pictures.Count == 0)
            {
                errorMessage = "minimum number of pictures is 1";
                error = true;
                return errorMessage;
            }
            var supportedTypes = new[] {"jpg","jpeg","png"};

            foreach (IFormFile picture in pictures)
            {
                string fileExt = System.IO.Path.GetExtension(picture.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    error = true;
                    errorMessage = "Format not accepted";
                }

                if (picture.Length > 10 * 1024 * 1024) // daca avem o imagine mai mare de 10MB
                {
                    error = true;
                    errorMessage = "Image(s) too large (6MB/picture limit)";
                }
            }

            if (!error)
            {
                (List<Stream>, Stream) imagesStream = await _imageManager.ProcessAsync(pictures.Select(i => new ImageInputModel
                {
                    Name = i.FileName,
                    Type = i.ContentType,
                    Content = i.OpenReadStream()
                }));
                return await _serviceRepo.CreateService(imagesStream.Item1,imagesStream.Item2, serviceModel, userName); // item1 pictures for slideshow
                                                                                                                        // item 2 thumbnail picture
            }
            else
            {
                return errorMessage;
            }
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

