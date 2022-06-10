using Microsoft.EntityFrameworkCore;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using rentalAppAPI.DAL.AWS.Interfaces;

namespace rentalAppAPI.DAL.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        public static Random random = new Random();
        

        public readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IS3Manager _s3Manager;
        public ServiceRepository(AppDbContext context, IConfiguration configuration, IS3Manager s3Manager)
        {
            _context = context;
            _configuration = configuration;
            _s3Manager = s3Manager;
        }

        public async Task<ServiceModel> ToServiceModel(Service serviceEntity)
        {
            var userEntity = await _context.Users.Where(x => x.Id == serviceEntity.UserId).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.RentalTypeId == serviceEntity.RentalTypeId).FirstOrDefaultAsync();
            //var picturesRepository = new PictureRepository();
            //ICollection<Picture> pictures = new List<Picture>();
            //pictures = await _context.Pictures.Where(x => x.IdService == serviceEntity.ServiceId).ToListAsync();
            ICollection<PictureModel> paths = new List<PictureModel>();
            string prefix = serviceEntity.IdentificationString + "/pic_";

            IEnumerable<S3ObjectDto> s3Objects = await _s3Manager.getPictures(prefix);
            
            foreach (var obj in s3Objects)
            {
                PictureModel picture = new PictureModel();
                picture.path = obj.PresignedUrl;
                paths.Add(picture);
            }
            
            var serviceModel = new ServiceModel
            {
                Title = serviceEntity.Title,
                Description = serviceEntity.Description,
                PhoneNumber = serviceEntity.PhoneNumber,
                Price = serviceEntity.Price,
                Username = userEntity.UserName,
                ServType = rentalEntity.Type,
                IdentificationString = serviceEntity.IdentificationString,
                PicturePaths = paths
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

            _s3Manager.deleteDirectory(IdentificationString);
            
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

        public async Task<List<ThumbnailServiceModel>> SearchServices(string serviceName)
        {
            List<Service> services = await _context.Services.Where(service => service.Title.Contains(serviceName)).ToListAsync();

            List<ThumbnailServiceModel> thumbnailServices = new List<ThumbnailServiceModel>();

            foreach (var service in services)
            {
                ThumbnailServiceModel thumbnailServiceModel = await ToThumbnailServiceModel(service);
                thumbnailServices.Add(thumbnailServiceModel);
            }

            return thumbnailServices;
        }

        public async Task<List<ThumbnailServiceModel>> RandomServices(int NumberOfServices)
        {
            List<int> ServicesId =  await _context.Services.Select(x => x.ServiceId).ToListAsync();
            var rnd = new Random();
            var x = ServicesId[rnd.Next(ServicesId.Count)];
            List<int> RandomIds = new List<int>();
            int index;
            for (index = 1; index <= NumberOfServices; index++)
            {
                RandomIds.Add(ServicesId[rnd.Next(ServicesId.Count)]);
            }
            List<Service> RandomServices = await _context.Services
                .Where(service =>
                    RandomIds.Contains(service.ServiceId)
                    )
                .ToListAsync();
            List<ThumbnailServiceModel> thumbnailServices = new List<ThumbnailServiceModel>();

            foreach (var service in RandomServices)
            {
                ThumbnailServiceModel thumbnailServiceModel = await ToThumbnailServiceModel(service);
                thumbnailServices.Add(thumbnailServiceModel);
            }

            return thumbnailServices;
        }

        private async Task<ThumbnailServiceModel> ToThumbnailServiceModel(Service serviceEntity)
        {
            var userEntity = await _context.Users.Where(x => x.Id == serviceEntity.UserId).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.RentalTypeId == serviceEntity.RentalTypeId).FirstOrDefaultAsync();
            PictureModel ThumbnailPath = new PictureModel();
            string prefix = serviceEntity.IdentificationString + "/thumbnail";
            IEnumerable<S3ObjectDto> s3Objects = await _s3Manager.getPictures(prefix); // this returns only one picture
            PictureModel picture = new PictureModel();

            if (s3Objects.Count() > 0) // daca nu este gasit un thumbnail va returna o poza nula
            {
                picture.path = s3Objects.FirstOrDefault().PresignedUrl;
            }
            ThumbnailPath = picture; 


            var serviceModel = new ThumbnailServiceModel()
            {
                Title = serviceEntity.Title,
                Price = serviceEntity.Price,
                Username = userEntity.UserName,
                ServType = rentalEntity.Type,
                IdentificationString = serviceEntity.IdentificationString,
                ThumbnailPath = ThumbnailPath
            };
            return serviceModel;
        }


        public async Task<string> CreateService(ICollection<Stream> pictures, Stream thumbnail, ServiceModelCreate serviceModel, string userName)
        {
            var userEntity = await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            var rentalEntity = await _context.RentalTypes.Where(x => x.Type == serviceModel.ServType).FirstOrDefaultAsync();
            //var picturesRepository = new PictureRepository();
            if ((userEntity == null) || (rentalEntity == null))
            {
                return "0";
            }
            string identificationString = RandomString(); // Represents the code of the ad

            //Console.Write(bucketName);


            string prefix = identificationString;
            int pictureIndex = 0;
            // add pictures
            foreach (Stream picture in pictures)
            {
                string picExtension = System.IO.Path.GetExtension(".jpeg").Substring(1);
                string newPictureName = "pic_" + pictureIndex.ToString() + "." +picExtension;
                pictureIndex++;
                
                await _s3Manager.addToS3(picture, newPictureName, prefix );
            }
            
            // add thumbnail
            
            string thumbnailExtension = System.IO.Path.GetExtension(".jpeg").Substring(1);
            string thumbnailName = "thumbnail." + thumbnailExtension;
            await _s3Manager.addToS3(thumbnail, thumbnailName, prefix + "/thumbnail");

            if ((userEntity != null) && (rentalEntity != null))
            {
                var service = new Service
                {
                    Title = serviceModel.Title,
                    Description = serviceModel.Description,
                    PhoneNumber = serviceModel.PhoneNumber,
                    Price = serviceModel.Price,
                    UserId = userEntity.Id,
                    RentalTypeId = rentalEntity.RentalTypeId,
                    IdentificationString = identificationString
                    //Pictures = picturesRepository.ToEntityList(serviceModel.Pictures)
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
