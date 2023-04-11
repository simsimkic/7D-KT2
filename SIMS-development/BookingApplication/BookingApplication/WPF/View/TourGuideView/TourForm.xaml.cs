using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using Image = BookingApplication.Domain.Model.Image;


namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>

    public partial class TourForm : Window
    {
        public Tour SelectedTour { get; set; }
        public Location SelectedLocation { get; set; }
        public Image SelectedThumbnailImage { get; set; }
        public static ObservableCollection<TourDate> TourDates { get; set; }

        public TourDate SelectedTourDate { get; set; }

        private readonly TourRepository _tourRepository;

        private readonly LocationRepository _locationRepository;

        private readonly ImageService _imageService;

        private readonly TourPointService _tourPointService;
        private readonly TourDateService _tourDateService;

        public List<string> Countries {get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public enum FormStatus {UPDATE, ADD, LIVE}

        public FormStatus Status { get; set; }

        public int UserId { get; set; }
        

        public TourForm(int userId)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Add Tour";

            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _imageService = new ImageService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();

          //  Countries = _locationRepository.GetLocations().Keys.ToList();

            SelectedTour = new Tour();
            SelectedTour.ImageId = new List<int>();

            SelectedLocation = new Location();
            SelectedThumbnailImage = new Image();
            
            TourDates = new ObservableCollection<TourDate>();
            UserId = userId;
            Status = FormStatus.ADD;
            ConfigureButtons();
        }

        public TourForm(Tour selectedTour, FormStatus status)
        {
            InitializeComponent();
            DataContext = this;
            
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _imageService = new ImageService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();

//            Countries = _locationRepository.GetLocations().Keys.ToList();

            SelectedTour = selectedTour;

            InitializeSelectedLocation();
            TourDates = new ObservableCollection<TourDate>(_tourDateService.GeTourDatesByTour(SelectedTour));
            Status = status;
            ConfigureButtons();
        }

        private void ConfigureButtons()
        {
            if (Status == FormStatus.ADD || Status == FormStatus.UPDATE)
            {
                EndTourButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = false;
            }
        }


        private void SaveTourClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour.IsValid)
            {
                ConfigureSelectedLocation();
                if (Status == FormStatus.UPDATE)
                {
                    Tour updatedTour = _tourRepository.Update(SelectedTour);
                    Location updatedLocation = _locationRepository.Update(SelectedLocation);
                    Image updatedImage = _imageService.Update(SelectedThumbnailImage);

                    if (updatedTour != null)
                    {
                        int index = TourOverview.Tours.IndexOf(SelectedTour);
                        TourOverview.Tours.Remove(SelectedTour);
                        TourOverview.Tours.Insert(index, updatedTour);
                        _tourPointService.SaveTourPointsInTour(updatedTour.Id);
                        Close();
                    }
                }
                else if (Status == FormStatus.ADD)
                {
                    SelectedTour.PossibleDates = TourDates.ToList();
                    Tour newTour = new Tour(SelectedTour.Name, SelectedTour.LocationId, SelectedTour.Description,
                        SelectedTour.Language, SelectedTour.MaxGuests, SelectedTour.PossibleDates, SelectedTour.Duration,
                        false, SelectedTour.ImageId, SelectedTour.ThumbnailUrl, UserId);
                    newTour = _tourRepository.Save(newTour);
                    TourOverview.Tours.Add(newTour);
                    _tourPointService.SaveTourPointsInTour(newTour.Id);
                    Close();
                }
            }
        }

        private void ConfigureSelectedLocation()
        {
            //checking if we already have this location stored in data
            if (_locationRepository.IsAlreadyInserted(SelectedLocation))
            {
                SelectedLocation.Id = _locationRepository.GetIdByCityAndCountry(SelectedLocation.City,
                    SelectedLocation.Country);
            }
            else
            {
                SelectedLocation = _locationRepository.Save(SelectedLocation);
            }

            SelectedTour.LocationId = SelectedLocation.Id;
        }
        
        private void CancelClick(object sender, EventArgs e)
        {
            DeleteTourPoints();
            Close();
        }

        private void DeleteTourPoints()
        {
            if (Status == FormStatus.ADD)
            {
                _tourPointService.DeleteUnassignedTourPoints();
            }
        }

        private void EndTourClick(object sender, EventArgs e)
        {
            //SelectedTour.Ended = true;
        }

        private void InitializeSelectedLocation()
        {
            Location initialLocation = _locationRepository.GetById(SelectedTour.LocationId);
            SelectedLocation = new Location(initialLocation.City, initialLocation.Country);
        }
        
        private void ShowGuideTourPointsOverview(object sender, RoutedEventArgs e)
        {
            if (Status == FormStatus.ADD || Status == FormStatus.UPDATE)
            {
                GuideTourPointOverview guideTourPointOverview = new GuideTourPointOverview(SelectedTour, false);
                guideTourPointOverview.Show();
            }
            else
            {
                GuideTourPointOverview guideTourPointOverview = new GuideTourPointOverview(SelectedTour, true);
                guideTourPointOverview.Show();
            }
        }

        private void ShowTourDateForm(object sender, RoutedEventArgs e)
        {
            if (SelectedTourDate != null)
            {
                TourDateForm tourDateForm = new TourDateForm(SelectedTour, SelectedTourDate,false);
                tourDateForm.Show();
            }
            else
            {
                TourDateForm tourDateForm = new TourDateForm(SelectedTour, false);
                tourDateForm.Show();
            }            
        }

        private void DeleteTourDate(object sender, RoutedEventArgs e)   
        {
            if (SelectedTourDate != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete" +
                                                          "?", "Delete Tour Date",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _tourDateService.DeleteTourDate(SelectedTourDate);
                    TourDates.Remove(SelectedTourDate);
                    SelectedTour.PossibleDates.Remove(SelectedTourDate);
                }
            }
        }
        
        private void ShowImagesOverview(object sender, RoutedEventArgs e)
        {
            TourImagesOverview tourImagesOverview = new TourImagesOverview(SelectedTour);
            tourImagesOverview.Show();

        }

        private void CountryComboBoxSelected(object sender, RoutedEventArgs e)
        {
            if (countryComboBox.SelectedIndex != -1)
            {
                cityComboBox.IsEnabled = true;
//                Cities = new ObservableCollection<string>(_locationRepository.GetLocations()[SelectedLocation.Country]);
                cityComboBox.ItemsSource = Cities;
            }
        }
        
        private void IncreaseMaxGuestsClick(object sender, RoutedEventArgs e)
        {
            SelectedTour.MaxGuests++;
        }

        private void DecreaseMaxGuestsClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour.MaxGuests!=0)
                SelectedTour.MaxGuests--;
        }
        private void IncreaseDurationClick(object sender, RoutedEventArgs e)
        {
            SelectedTour.Duration++;
        }

        private void DecreaseDurationClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour.Duration != 0)
                SelectedTour.Duration--;
        }


    }
}
