#nullable enable
namespace rentalAppAPI.DAL.Models;

public class S3ObjectDto
{
    public string? Name { get; set; }
    public string? PresignedUrl { get; set; }
}