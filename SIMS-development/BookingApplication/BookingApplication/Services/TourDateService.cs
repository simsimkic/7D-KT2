using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class TourDateService
    {
        private readonly ITourDateRepository _tourDateRepository;

        public TourDateService()
        {
            _tourDateRepository = Injector.CreateInstance<ITourDateRepository>();
        }

        public void DeleteTourDate(TourDate tourDate)
        {
            _tourDateRepository.Delete(tourDate);
        }

        public void SaveTourDate(TourDate tourDate)
        {
            _tourDateRepository.Save(tourDate);
        }

        public TourDate UpdateTourDate(TourDate tourDate)
        {
            return _tourDateRepository.Update(tourDate);
        }

        public List<TourDate> GeTourDatesByTour(Tour tour)
        {
            List<TourDate> tourDates = new List<TourDate>();
            foreach (var tourDate in _tourDateRepository.GetAll())
            {
                if (tourDate.TourId == tour.Id)
                {
                    tourDates.Add(tourDate);
                }
            }
            return tourDates;
        }


    }
}
