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
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location Save(Location location)
        {
            return _locationRepository.Save(location);
        }

        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }

        public Dictionary<string, List<string>> GetLocations()
        {
            var locations = _locationRepository.GetAll();
            Dictionary<string, List<string>> countryCities = locations
                .GroupBy(l => l.Country)
                .ToDictionary(g => g.Key, g => g.Select(l => l.City).ToList());
            return countryCities;
        }

        public bool IsTourOnLocation(Location location, Tour tour)
        {
            var tourLocation = _locationRepository.GetById(tour.LocationId);
            return (tourLocation.Country == location.Country || string.IsNullOrEmpty(location.Country)) &&
                   (tourLocation.City == location.City || string.IsNullOrEmpty(location.City));

        }
    }
}
