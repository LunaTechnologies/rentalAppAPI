using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Models
{
    public class ServiceModel
    {
        public int ServiceId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public int Price { get; set; }

        public int UserId { get; set; }

        public int RentalTypeId { get; set; }

        public string IdentificationString { get; set; }
    }
}
