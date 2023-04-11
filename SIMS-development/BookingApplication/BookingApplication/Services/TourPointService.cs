using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Serializer;

namespace BookingApplication.Services
{
    public class TourPointService
    {
        private readonly ITourPointRepository _tourPointRepository;
        public TourPointService()
        {
            _tourPointRepository = Injector.CreateInstance<ITourPointRepository>();

        }

        public void DeleteTourPoint(TourPoint tourPoint)
        {
            _tourPointRepository.Delete(tourPoint);
        }

        public void SaveTourPoint(TourPoint tourPoint)
        {
            _tourPointRepository.Save(tourPoint);
        }

        public TourPoint UpdateTourPoint(TourPoint tourPoint)
        {
            return _tourPointRepository.Update(tourPoint);
        }

        public List<TourPoint> GetAllTourPoints()
        {
            return _tourPointRepository.GetAll();
        }

        public List<TourPoint> GetTourPointsByTour(Tour tour)
        {

            List<TourPoint> tourPoints = new List<TourPoint>();
            foreach (var tourPoint in _tourPointRepository.GetAll())
            {
                if (tourPoint.TourId == tour.Id)
                {
                    tourPoints.Add(tourPoint);
                }
            }
            return tourPoints;
        }

        public void DeleteUnassignedTourPoints()
        {
            List<TourPoint> tourPoints = _tourPointRepository.GetAll();
            int tourPointsLength = tourPoints.Count;
            for (int i = 0; i < tourPointsLength; i++)
            {
                if (tourPoints[i].TourId == 0)
                {
                    tourPoints.Remove(tourPoints[i]);
                }
            }
            _tourPointRepository.SaveAll(tourPoints);

        }

        public void SaveTourPointsInTour(int tourId)
        {
            foreach (TourPoint tourPoint in _tourPointRepository.GetAll())
            {
                if (tourPoint.TourId == 0)
                {
                    tourPoint.TourId = tourId;
                    _tourPointRepository.Update(tourPoint);
                }
            }
        }
    }
}
