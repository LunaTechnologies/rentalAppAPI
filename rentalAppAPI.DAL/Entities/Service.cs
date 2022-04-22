using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Entities
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public int Price { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int RentalTypeId { get; set; }

        public virtual RentalType RentalType { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

    }
}
