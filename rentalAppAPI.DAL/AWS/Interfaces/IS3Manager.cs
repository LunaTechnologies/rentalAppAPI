using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using rentalAppAPI.DAL.Models;

namespace rentalAppAPI.DAL.AWS.Interfaces;

public interface IS3Manager
{
    public Task addToS3(Stream picture, String pictureName, String prefix);
    public Task deleteDirectory(string identificationString);
    public Task<IEnumerable<S3ObjectDto>> getPictures(string prefix);

}