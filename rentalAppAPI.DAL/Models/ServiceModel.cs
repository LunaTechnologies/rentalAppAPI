using rentalAppAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Models
{
    public class ServiceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public int Price { get; set; }
        
        public string Username { get; set; }

        public string ServType { get; set; }

        public virtual ICollection<PictureModel> Pictures { get; set; }
    }
}
