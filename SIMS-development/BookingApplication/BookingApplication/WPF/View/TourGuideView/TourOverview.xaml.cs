using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class TourOverview : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourRepository _tourRepository;
        private readonly  VoucherService _voucherService;
        public TourOverview()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _voucherService = new VoucherService();
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
        }

        private void ShowAddTourForm(object sender, RoutedEventArgs e)
        {
            //temporarily default userId will be six
            TourForm createTourForm = new TourForm(6);
            createTourForm.Show();
        }

        private void ShowLiveTourForm(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null && SelectedTour.Began)
            {
                TourForm tourForm = new TourForm(SelectedTour, TourForm.FormStatus.LIVE);
                tourForm.Show();
            }
        }

        private void ShowUpdateTourForm(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                TourForm tourForm = new TourForm(SelectedTour, TourForm.FormStatus.UPDATE);
                tourForm.Show();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _voucherService.AssignVouchersToToursGuests(SelectedTour);
                    _tourRepository.Delete(SelectedTour);
                    Tours.Remove(SelectedTour);
                }
            }
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {
            SelectedTour.Began = true;
            _tourRepository.Update(SelectedTour);
        }


    }
}
