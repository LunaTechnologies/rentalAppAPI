using System.Collections.Generic;

namespace rentalAppAPI.DAL.Models;

public class ServiceModelCreate
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public int Price { get; set; }
    
    public string Username { get; set; }

    public string ServType { get; set; }

    public virtual ICollection<PictureModel> Pictures { get; set; }
}