using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.BLL.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace rentalAppAPI.BLL.Managers;

public class ImageManager : IImageManager
{
    private const int ThumbnailWidth = 200;
    private const int SlideShowWidth = 500;
    private const int FullscreenWidth = 1000;
    public async Task< ( List<Stream>, Stream ) > ProcessAsync(IEnumerable<ImageInputModel> images)
    {
        List<Stream> imagesStreams = new List<Stream>();
        Stream thumbnail = new MemoryStream();
        Boolean firstPicture = false;
        var tasks = images
            .Select(image => Task.Run (async() =>
        {
            try
            {
                using var imageResult = await Image.LoadAsync(image.Content);

                //await this.SaveImage(imageResult, $"Original_{image.Name}", imageResult.Width);
                //await this.SaveImage(imageResult, $"Fullscreen_{image.Name}", imageResult.Width);
                Stream imageStream = await this.SaveImage(imageResult, $"slideshow_{image.Name}",75, SlideShowWidth);
                
                
                imagesStreams.Add(imageStream);
                
                if (!firstPicture)
                {
                    thumbnail = await this.SaveImage(imageResult, $"thumbnail_{image.Name}",70 , ThumbnailWidth);
                    
                    firstPicture = true;
                }
                
            }
            catch
            {
                // Log information.
            }
        }))
        .ToList();
            


        await Task.WhenAll(tasks);
        
        return (imagesStreams, thumbnail);
    }

    private async Task<Stream> SaveImage(Image image, string name, int quality, int resizeWidth)
    {
        var width = image.Width;
        var height = image.Height;
        if (width > resizeWidth)
        {
            height = (int)((double)resizeWidth / width * height);
            width = resizeWidth;
        }
            
        image
            .Mutate(i 
                => i.Resize(new Size(width,height)));
        image.Metadata.ExifProfile = null;
        Stream imageStream = new MemoryStream();
        await image.SaveAsJpegAsync(imageStream, new JpegEncoder
        {
            Quality = quality
        });
        imageStream.Seek(0, SeekOrigin.Begin);
        //

        return imageStream;
    }
}