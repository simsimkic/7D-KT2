using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverview : Page
    {
        
        public TourOverviewVm TourOverviewVm { get; set; }
        public ToursOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            TourOverviewVm = new TourOverviewVm(user, navigationService);
            DataContext = TourOverviewVm;
            
        }
    }
}
