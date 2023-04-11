using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Services
{
    class TourReservationService
    {
        private readonly ITourReservationRepository _tourReservationRepository;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
        }

        public void DeleteTourReservation(TourReservation tourReservation)
        {
            _tourReservationRepository.Delete(tourReservation);
        }

        public void SaveTourReservation(TourReservation tourReservation)
        {
            _tourReservationRepository.Save(tourReservation);
        }

        public TourReservation UpdateTourReservation(TourReservation tourReservation)
        {
            return _tourReservationRepository.Update(tourReservation);
        }

        public List<TourReservation> GetAllTourReservations()
        {
            return _tourReservationRepository.GetAll();
        }

    }
}
