using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for GuestTourOverview.xaml
    /// </summary>
    public partial class GuestTourOverview : Window
    {
        public User User { get; set; }
        public NavigationService NavigationService { get; set; }
        public GuestTourOverview(User user)
        {
            InitializeComponent();
            User = user;
            NavigationService = new NavigationService(ToursWindow);
            ToursWindow.Content = new ToursOverview(user, NavigationService);
        }

        private void OverviewButton_OnClick(object sender, RoutedEventArgs e)
        {
            //ToursWindow.Content = new ToursOverview(User);
            //NavigationService.GetNavigationService(new ToursOverviewPage(User));
            NavigationService.NavigateTo(new ToursOverview(User, NavigationService));
        }
    }
}
