using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationPage : Page
    {
        public Tour SelectedTour { get; set; }
        public int GroupSize { get; set; }
        public List<Image> Images { get; set; }
        public string CurrentUrl { get;set; }

        public LocationRepository locationRepository;
        public User User { get; set; }
        public List<TourDate> TourDates { get; set; }
        public ObservableCollection<TourReservation> TourReservations { get;set; }
        public List<DateTime> SelectedDates { get; set; }
        public List<string> Dates { get; set; }
        public Location TourLocation { get; set; }
        public ImageService ImageService;
        public DateTime? SelectedDate { get; set; }
        public TourReservationPage(Tour selectedTour, User user)
        {
            InitializeComponent();
            
            DataContext = this;
            DisableAlternativeView();
            ImageService = new ImageService();
            CurrentUrl = ImageService.GetImageUrl(ImageService.GetAll()[0].Id);
            //ShowImagesOnStartup();
            SelectedTour = new Tour();
            SelectedTour = selectedTour;
            User = new User();
            User = user;
            locationRepository = new LocationRepository();
            TourLocation = locationRepository.GetById(SelectedTour.LocationId);
            TourDates = SelectedTour.PossibleDates;
            TourReservations = new ObservableCollection<TourReservation>();
            SelectedDates = new List<DateTime>();
            SelectedDates = SelectedTour.PossibleDates.Select(date => date.StartTime).ToList();
            Dates = new List<string>();
            Dates = PossibleDatesToString(SelectedDates);
            DatesComboBox.ItemsSource = Dates;
        }

        

        private void DisableAlternativeView()
        {
            ConfirmationGroupBox.Visibility = Visibility.Hidden;
            BookTourButton.IsEnabled = false;
            AvailableButton.IsEnabled = false;
            ToursDataGrid.Visibility = Visibility.Hidden;
        }

        private List<string> PossibleDatesToString(List<DateTime> possibleDates)
        {
            var dates = new List<string>();
            foreach (var date in possibleDates)
            {
                dates.Add(date.ToString("dd/MM/yyyy"));
            }
            return dates;
        }

        private ObservableCollection<Tour> GetAlternativeToursOnLocation(int locationId, int tourId)
        {
            var tourRepository = new TourRepository();
            var tours = new ObservableCollection<Tour>();
            foreach (var tour in tourRepository.GetAll())
            {
                if (tour.LocationId == locationId && tour.Id != tourId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }

        private void CancelTourButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGroupBox.Visibility = Visibility.Hidden;
            DatesComboBox.SelectedIndex = -1;
            GroupSizeTextBox.Text = "";
            AvailableButton.IsEnabled = false;

        }
        private int CalculateAvailableGroupSize(int groupSize)
        {
            return SelectedTour.MaxGuests - SelectedTour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests;
        }

        private bool IsSizeAvailable(int groupSize, Tour tour)
        {
            return groupSize <= tour.MaxGuests - tour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests;
        }

        private bool IsTourFilled(Tour tour)
        {
            return tour.MaxGuests - tour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests == 0;
        }
        private void AvailableButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsTourFilled(SelectedTour))
            {
                WarningTextBlock.Text = "The selected tour is full. Select alternative from below.";
                BookTourButton.IsEnabled = false;
                ToursDataGrid.Visibility = Visibility.Visible;
                ToursDataGrid.ItemsSource = GetAlternativeToursOnLocation(TourLocation.Id, SelectedTour.Id);
                return;
            }
            if (IsSizeAvailable(GroupSize, SelectedTour))
            {
                BookTourButton.IsEnabled = true;
                WarningTextBlock.Text = "";
                ConfirmationGroupBox.Visibility = Visibility.Visible;
            }
            else
            {
                BookTourButton.IsEnabled = false;
                WarningTextBlock.Text =" Maximum available size at the moment is: " + CalculateAvailableGroupSize(GroupSize);
            }
        }

        private void NextImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            MyImage.Source = new BitmapImage(new Uri(ImageService.ChangeImageForward()));
        }

        private void PreviousImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            MyImage.Source = new BitmapImage(new Uri(ImageService.ChangeImageBackward()));
        }

        private void BookTour(int userId, int tourId, DateTime date, int groupSize)
        {
            var tourReservation = new TourReservation(userId, tourId, groupSize, date);
            SelectedTour.PossibleDates[0].CurrentGuests += groupSize;
            var tourDateRepository = new TourDateRepository();
            tourDateRepository.Update(SelectedTour.PossibleDates.Find(d => d.StartTime == SelectedDate));
            var tourReservationRepository = new TourReservationRepository();
            TourReservations.Add(tourReservation);
            tourReservationRepository.Save(tourReservation);
        }

        private void DatesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DatesComboBox.SelectedIndex != -1)
                SelectedDate = SelectedDates[DatesComboBox.SelectedIndex];
            if(!string.IsNullOrEmpty(GroupSizeTextBox.Text))
                AvailableButton.IsEnabled = true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void GroupSizeTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(DatesComboBox.SelectedIndex != -1)
                AvailableButton.IsEnabled = true;
        }

        private void BookTourButton_OnClick(object sender, RoutedEventArgs e)
        {
            BookTour(User.Id, SelectedTour.Id, SelectedTour.PossibleDates[0].StartTime, GroupSize);
            //NavigationService.Navigate(new ToursOverview(User));
        }

        private void ReserveButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TourReservationPage(SelectedTour, User));
        }
    }
}
