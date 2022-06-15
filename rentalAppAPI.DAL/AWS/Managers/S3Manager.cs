using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using rentalAppAPI.DAL.AWS.Interfaces;
using rentalAppAPI.DAL.Models;

namespace rentalAppAPI.DAL.AWS;

public class S3Manager : IS3Manager
{
    private readonly string bucketName = "quide.testing.bucket";
    private readonly IAmazonS3 _s3Client;
    
    public S3Manager(IAmazonS3 s3Client)
    {
        _s3Client = s3Client; 
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
        await _s3Client.PutObjectAsync(request);
    }

    public async Task deleteDirectory(string identificationString)
    {
        DeleteObjectsRequest deleteRequest = new DeleteObjectsRequest();
        deleteRequest.BucketName = bucketName;

        ListObjectsRequest request = new ListObjectsRequest()
        {
            BucketName = bucketName,
            Prefix = identificationString
        };
        ListObjectsResponse response = await _s3Client.ListObjectsAsync(request);
        foreach (S3Object entry in response.S3Objects)
        {
            deleteRequest.AddKey(entry.Key);
        }

        DeleteObjectsResponse deleteResponse = await _s3Client.DeleteObjectsAsync(deleteRequest);
    }

    public async Task<IEnumerable<S3ObjectDto>> getPictures(string prefix)
    {
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
        return s3Objects;
    }
}