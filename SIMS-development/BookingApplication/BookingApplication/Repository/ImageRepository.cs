﻿
using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class ImageRepository : IImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> _serializer;

        private List<Image> images;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            images = _serializer.FromCSV(FilePath);
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Image Save(Image image)
        {
            image.Id = NextId();
            images = _serializer.FromCSV(FilePath);
            images.Add(image);
            _serializer.ToCSV(FilePath, images);
            return image;
        }

        public int NextId()
        {
            images = _serializer.FromCSV(FilePath);
            if (images.Count < 1)
            {
                return 1;
            }

            return images.Max(c => c.Id) + 1;
        }

        public void Delete(Image image)
        {
            images = _serializer.FromCSV(FilePath);
            Image founded = images.Find(c => c.Id == image.Id);
            images.Remove(founded);
            _serializer.ToCSV(FilePath, images);
        }

        public Image Update(Image image)
        {
            images = _serializer.FromCSV(FilePath);
            Image current = images.Find(c => c.Id == image.Id);
            int index = images.IndexOf(current);
            images.Remove(current);
            images.Insert(index, image); // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, images);
            return image;
        }

        public Image GetById(int id)
        {
            foreach (var image in images)
            {
                if (image.Id == id) return image;
            }
            return null;
        }
    }
}