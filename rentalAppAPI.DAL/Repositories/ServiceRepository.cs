using Microsoft.EntityFrameworkCore;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace rentalAppAPI.DAL.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        public static Random random = new Random();
        
        private readonly IAmazonS3 _s3Client;

        public readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string bucketName = "quide.testing.bucket";
        public ServiceRepository(AppDbContext context, IAmazonS3 s3Client, IConfiguration configuration)
        {
            _context = context;
            _s3Client = s3Client;
            _configuration = configuration;
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
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            
            var result = await _s3Client.ListObjectsV2Async(request);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                return new S3ObjectDto()
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                };
            });

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
            
            DeleteObjectsRequest deleteRequest = new DeleteObjectsRequest();
            deleteRequest.BucketName = bucketName;

            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = bucketName,
                Prefix = IdentificationString
            };
            ListObjectsResponse response = await _s3Client.ListObjectsAsync(request);
            foreach (S3Object entry in response.S3Objects)
            {
                deleteRequest.AddKey(entry.Key);
            }

            DeleteObjectsResponse deleteResponse = await _s3Client.DeleteObjectsAsync(deleteRequest);
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

        public async Task addToS3(Stream picture, String pictureName, String prefix)
        {
            string picExtension = System.IO.Path.GetExtension(".jpeg").Substring(1);
            string newPictureName = pictureName;
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix)
                    ? newPictureName
                    : $"{prefix?.TrimEnd('/')}/{newPictureName}",
                InputStream = picture
            };
            //request.Metadata.Add("Content-Type", picture);
            await _s3Client.PutObjectAsync(request);
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
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return "bucket not found";

            string prefix = identificationString;
            int pictureIndex = 0;
            // add pictures
            foreach (Stream picture in pictures)
            {
                string picExtension = System.IO.Path.GetExtension(".jpeg").Substring(1);
                string newPictureName = "pic_" + pictureIndex.ToString() + "." +picExtension;
                pictureIndex++;

                await addToS3(picture, newPictureName, prefix );
            }
            
            // add thumbnail
            
            string thumbnailExtension = System.IO.Path.GetExtension(".jpeg").Substring(1);
            string thumbnailName = "thumbnail." + thumbnailExtension;
            await addToS3(thumbnail, thumbnailName, prefix + "/thumbnail");

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
