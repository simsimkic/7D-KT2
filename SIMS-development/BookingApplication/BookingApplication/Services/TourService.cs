using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;

namespace BookingApplication.Services
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour Save(Tour tour)
        {
            return _tourRepository.Save(tour);
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        public ObservableCollection<Tour> FilterTours(TourFilterDto tourFilterDto)
        {
            var tours = _tourRepository.GetAll();
            var filteredTours = new ObservableCollection<Tour>();
            var locationService = new LocationService();

            foreach (var tour in tours)
            {
                var isLanguage = string.IsNullOrEmpty(tourFilterDto.Language) || tour.Language.ToLower().Contains(tourFilterDto.Language.ToLower());
                var isDuration = tourFilterDto.Duration is null || tourFilterDto.Duration == tour.Duration;
                var isGroupSize = tourFilterDto.GroupSize is null || tourFilterDto.GroupSize <= tour.MaxGuests;
                var isLocation = locationService.IsTourOnLocation(tourFilterDto.Location, tour);

                if (isLanguage && isDuration && isGroupSize && isLocation)
                {
                    filteredTours.Add(tour);
                }
            }

            return filteredTours;
        }

    }
}
