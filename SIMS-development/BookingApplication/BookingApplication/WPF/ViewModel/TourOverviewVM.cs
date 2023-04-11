using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using BookingApplication.WPF.View.TourGuestView;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.ViewModel
{
    public class TourOverviewVm : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public TourFilterDto TourFilterDto { get; set; }


        private readonly LocationService _locationService;

        private readonly TourService _tourService;
        public ICommand ResetAllCommand { get; set; }
        public ICommand BookTourCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand CountryChangedCommand { get; set; }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedCountryIndex = -1;

        public int SelectedCountryIndex
        {
            get => _selectedCountryIndex;
            set
            {
                if (_selectedCountryIndex != value)
                {
                    _selectedCountryIndex = value;
                    OnPropertyChanged();
                }
            }
        }


        private int _selectedCityIndex = -1;

        public int SelectedCityIndex
        {
            get => _selectedCityIndex;
            set
            {
                if (_selectedCityIndex != value)
                {
                    _selectedCityIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _language;

        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly NavigationService _navigationService;
        public User User { get; set; }

        public TourOverviewVm(User user, NavigationService navigationService)
        {
            _navigationService = navigationService;
            IsEnabled = false;
            User = new User();
            User = user;
            _locationService = new LocationService();
            var languageService = new LanguageService();
            _tourService = new TourService();
            TourFilterDto = new TourFilterDto
            {
                Location = new Location()
            };
            SelectedTour = new Tour();
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Countries = _locationService.GetLocations().Keys.ToList();
            Languages = languageService.GetLanguages();
            ResetAllCommand = new RelayCommand(ResetAll);
            FilterCommand = new RelayCommand(FilterTours);
            BookTourCommand = new RelayCommand(BookTour);
            Cities = new ObservableCollection<string>();
            CountryChangedCommand = new RelayCommand(CountryChanged);
        }

        public void ResetAll()
        {
            TourFilterDto.Duration = null;
            TourFilterDto.GroupSize = null;
            TourFilterDto.Language = "";
            TourFilterDto.Location.Country = "";
            TourFilterDto.Location.City = "";
            SelectedCountryIndex = -1;
            SelectedCityIndex = -1;
            IsEnabled = false;
            Tours.Clear();
            foreach (var tour in _tourService.GetAll())
            {
                Tours.Add(tour);
            }

        }
        public void CountryChanged()
        {

            if (SelectedCountryIndex != -1)
            {
                IsEnabled = true;
                Cities.Clear();
                foreach (var city in _locationService.GetLocations()[TourFilterDto.Location.Country])
                {
                    Cities.Add(city);
                }

            }
        }

        public void FilterTours()
        {
            Tours.Clear();
            foreach (var tour in _tourService.FilterTours(TourFilterDto))
            {
                Tours.Add(tour);
            }
        }




        public  void BookTour()
        {
            /*var reservation = new TourReservationPage(SelectedTour, User);
            NavigationService ns = new NavigationService();
            ns.Navigate(reservation);*/
            _navigationService.NavigateTo(new TourReservationPage(SelectedTour, User));
            //var navigationService = NavigationService.GetNavigationService(this);

            // Navigate to a new page using the instance of the NavigationService
            //navigationService.Navigate(new Uri("SecondPage.xaml", UriKind.Relative));

            //NavigationService.Navigate(new TourReservationPage(SelectedTour, User));
            //ns.Navigate(new TourReservationPage(SelectedTour, User));
            
            //TourReservationPage tourReservationPage = new TourReservationPage(SelectedTour, User);
            //NavigationService.Navigate(tourReservationPage);
        }
    }
}
