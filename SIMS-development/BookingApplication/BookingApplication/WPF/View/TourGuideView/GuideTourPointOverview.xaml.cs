using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourPointOverview.xaml
    /// </summary>
    public partial class GuideTourPointOverview : Window
    {
        public static ObservableCollection<TourPoint>? TourPoints { get; set; }
        public static ObservableCollection<TourReservation>? TourReservations { get; set; }
        public Tour SelectedTour { get; set; }
        public TourPoint SelectedTourPoint { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        
        private readonly TourReservationRepository _tourReservationRepository;

        private readonly TourPointService _tourPointService;

        
        private bool _isLive;

        public bool IsLive
        {
            get { return _isLive; }
            set
            {
                if (_isLive != value)
                {
                    _isLive = value;
                }
            }
        }

        private static int _finishedTourPointsCount;

        public static int FinishedTourPointsCount
        {
            get { return _finishedTourPointsCount; }
            set
            {
                if (_finishedTourPointsCount != value)
                {
                    _finishedTourPointsCount = value;
                }
            }
        }

        public GuideTourPointOverview(Tour selectedTour, bool isLive)
        {
            InitializeComponent();
            DataContext = this;

            _tourReservationRepository = new TourReservationRepository();

            _tourPointService = new TourPointService();

            IsLive = isLive;
            SelectedTour = selectedTour;
            SelectedTourPoint = new TourPoint();
            SelectedTourReservation = new TourReservation();

            TourPoints = new ObservableCollection<TourPoint>(_tourPointService.GetTourPointsByTour(SelectedTour));
            FinishedTourPointsCount = GetFinishedTourPointsCount();

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            
        }
        
        private void ShowAddTourPointForm(object sender, RoutedEventArgs e)
        {
            TourPointForm createTourPointForm = new TourPointForm(SelectedTour);
            createTourPointForm.Show();
        }

        private void ShowUpdateTourPointForm(object sender, RoutedEventArgs e)
        {
            if (SelectedTourPoint != null)
            {
                TourPointForm updateTourPointForm = new TourPointForm(SelectedTour, SelectedTourPoint);
                updateTourPointForm.Show();
            }
        }
        

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _tourPointService.DeleteTourPoint(SelectedTourPoint);
                    TourPoints.Remove(SelectedTourPoint);
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if (TourPoints.Count < 2)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to exit Tour Point Form? You haven't input" +
                                                          "atleast two Tour Points.",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    DeleteRemainingTourPoint();
                    Close();
                }
                else
                {
                    return;
                }
            }

            Close();
        }

        private void DeleteRemainingTourPoint()
        {
            if (TourPoints.Count == 1)
            {
                _tourPointService.DeleteTourPoint(TourPoints[0]);
            }
            TourPoints.Clear();
        }




        private void ActivateTourPointClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTourPoint == null)
            {
                MessageBox.Show("Tour Point not selected!");
            }
            else
            {
                if (SelectedTourPoint.Order == FinishedTourPointsCount + 1 && SelectedTourPoint.Active == false)
                {
                    TourPointMarkGuests tourPointMarkGuests = new TourPointMarkGuests(SelectedTourPoint);
                    tourPointMarkGuests.Show();
                }
                else
                {
                    if (SelectedTourPoint.Order <= FinishedTourPointsCount)
                    {
                        MessageBox.Show("Tour Point already activated.");
                    }
                    else
                    {
                        MessageBox.Show("Previous Tour Points have not been activated.");
                    }
                }
            }
        }

        private int GetFinishedTourPointsCount()
        {
            int finishedTourPointsCount = 0;
            foreach (var tourPoint in TourPoints)
            {
                if (tourPoint.Active)
                {
                    finishedTourPointsCount++;
                }
            }
            return finishedTourPointsCount;
        }
    }
}





