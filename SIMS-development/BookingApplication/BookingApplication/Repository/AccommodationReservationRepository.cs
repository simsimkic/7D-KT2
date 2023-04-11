using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{

    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/acc_reservation.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservation;

        public AccommodationReservationRepository()
        {

            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservation = _serializer.FromCSV(FilePath);

        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.ReservationId = NextId();
            _accommodationReservation = _serializer.FromCSV(FilePath);
            _accommodationReservation.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservation);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            if (_accommodationReservation.Count < 1)
            {
                return 1;
            }

            return _accommodationReservation.Max(ac => ac.ReservationId) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservation.Find(ac => ac.ReservationId == accommodationReservation.ReservationId);
            _accommodationReservation.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservation.Find(ac => ac.ReservationId == accommodationReservation.ReservationId);
            int index = _accommodationReservation.IndexOf(current);
            _accommodationReservation.Remove(current);
            _accommodationReservation.Insert(index, accommodationReservation); // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _accommodationReservation);

            return accommodationReservation;
        }

    }

}