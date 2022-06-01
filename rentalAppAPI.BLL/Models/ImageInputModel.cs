using System.IO;

namespace rentalAppAPI.BLL.Models;

public class ImageInputModel
{
    public string Name { get; set; }
    public string Type { get; set; }
    public Stream Content { get; set; }
}