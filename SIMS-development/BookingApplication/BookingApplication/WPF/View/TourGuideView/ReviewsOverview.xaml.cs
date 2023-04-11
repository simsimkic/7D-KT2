using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for ReviewsOverview.xaml
    /// </summary>

    public partial class ReviewsOverview : Window
    {
        public ObservableCollection<Tour> Tours { get; set; }
        


        public ReviewsOverview()
        {
            InitializeComponent();
            DataContext = this;

            

        }
    }
}
