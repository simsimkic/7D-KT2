using System.Windows;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for AccommodationsOverallView.xaml
    /// </summary>
    public partial class AccommodationsOverallView : Window
    {
        public AccommodationsOverallView()
        {
            InitializeComponent();
            DataContext = this;
        }


        private void ShowOverviewForm(object sender, RoutedEventArgs e)
        {
            GuestAccommodationsOverview createShowOverviewForm = new GuestAccommodationsOverview();
                createShowOverviewForm.Show();
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
    }
}
