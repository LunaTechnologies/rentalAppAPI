using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Entities
{
    public class Picture
    {
        [Key]
        public int IdPicture { get; set; }
        public string Path { get; set; }
        public int IdService { get; set; }
        public virtual Service Service { get; set; }

    }
}
