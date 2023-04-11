using System;
using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for GuestReservationAccommodation.xaml
    /// </summary>
    public partial class GuestReservationAccommodation : Window
    {
        public Accommodation SelectedAccommodation { get; set; }
        public Guest Guest { get; set; }

        public GuestRepository _guestRepository { get; set; }

        public AccommodationRepository _accommodationRepository { get; set; }

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }

        public int GuestLimit { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public GuestReservationAccommodation()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReserveButton_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void CheckAvailabilityButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
