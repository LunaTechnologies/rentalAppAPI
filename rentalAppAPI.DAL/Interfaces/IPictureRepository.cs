using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Interfaces
{
    public interface IPictureRepository
    {
        public PictureModel ToPictureModel(Picture pictureEntity);
        public ICollection<PictureModel> ToPictureModelList(ICollection<Picture> picturesEntity);
        public Picture ToEntity(PictureModel pictureModel);
        public ICollection<Picture> ToEntityList(ICollection<PictureModel> picturesModel);
    }
}
