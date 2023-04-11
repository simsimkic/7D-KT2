using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guests.csv";

        private readonly Serializer<Guest> _serializer;

        private List<Guest> _guests;

        public GuestRepository()
        {
            _serializer = new Serializer<Guest>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public List<Guest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _guests = _serializer.FromCSV(FilePath);
            return _guests.FirstOrDefault(u => u.Username == username);
        }

       
    }
}