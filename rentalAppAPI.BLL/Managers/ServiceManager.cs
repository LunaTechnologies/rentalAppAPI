﻿using rentalAppAPI.BLL.Interfaces;
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
            var supportedTypes = new[] {"jpg","jpeg","png"};
            Boolean error = false;
            string errorMessage = "";
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
            // process the images
            
            ICollection<Stream> imagesStream = await _imageManager.ProcessAsync(pictures.Select(i => new ImageInputModel
            {
                Name = i.FileName,
                Type = i.ContentType,
                Content = i.OpenReadStream()
            }));
            
            //var memoryStream = new MemoryStream();
            
            //pictures.First().CopyTo(memoryStream);
            //memoryStream.ToArray();
            //
            if (!error)
                return await _serviceRepo.CreateService(imagesStream, serviceModel, userName);
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

