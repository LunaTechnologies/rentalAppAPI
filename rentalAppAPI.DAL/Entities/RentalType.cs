using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Entities
{
    public class RentalType
    {
        [Key]
        public int RentalTypeId { get; set; }

        public string  Type { get; set; }
        
        public virtual ICollection<Service> Services { get; set; }
    }
}
