namespace rentalAppAPI.DAL.Models;

public class ThumbnailServiceModel
{
    public string Title { get; set; }
    public int Price { get; set; }
    public string IdentificationString { get; set; }
    public string Username { get; set; }
    public string ServType { get; set; }
    public PictureModel ThumbnailPath { get; set; }
}