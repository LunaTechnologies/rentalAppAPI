using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using rentalAppAPI.BLL.Models;
using SixLabors.ImageSharp;

namespace rentalAppAPI.BLL.Interfaces;

public interface IImageManager
{
    public Task<(List<Stream>, Stream)> ProcessAsync(IEnumerable<ImageInputModel> images);
}