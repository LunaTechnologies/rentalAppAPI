using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        public Picture ToEntity(PictureModel pictureModel)
        {
            var picture = new Picture
            {
                Path = pictureModel.Path
            };
            return picture;
        }

        public ICollection<Picture> ToEntityList(ICollection<PictureModel> picturesModel)
        {
            ICollection<Picture> picturesEntity = new List<Picture>();
            foreach (PictureModel pictureModel in picturesModel)
            {
                picturesEntity.Add(ToEntity(pictureModel));
            }
            return picturesEntity;
        }

        public PictureModel ToPictureModel(Picture pictureEntity)
        {
            var pictureModel = new PictureModel
            {
                Path = pictureEntity.Path
            };
            return pictureModel;
        }

        public ICollection<PictureModel> ToPictureModelList(ICollection<Picture> picturesEntity)
        {
            ICollection<PictureModel> picturesModel = new List<PictureModel>();
            foreach (Picture picture in picturesEntity)
            {
                picturesModel.Add(ToPictureModel(picture));
            }
            return picturesModel;
        }

    }
}
