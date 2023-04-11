using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GuestAccommodationsOverview : Window
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }

        private readonly AccommodationRepository _accommodationsRepository;
        private readonly LocationRepository _locationRepository;
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }
        public Location SelectedLocation { get; set; }
        public int? SelectedMaxGuest { get; set; }
        public string SelectedName { get; set; }
        public AccommodationType SelectedType { get; set; }
        public int? SelectedMinReservationDays { get; set; }


        public GuestAccommodationsOverview()
        {
            /*
            InitializeComponent();
            this.DataContext = this;
            _accommodationsRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationsRepository.GetAll());
            Countries = _locationRepository.GetLocations().Keys.ToList();
            CityComboBox.IsEnabled = false;
            SelectedLocation = new Location();

            /* Types= new ObservableCollection<string>();
             Types.Add("");
             Types.Add("House");
             Types.Add("Shack");
             Types.Add("Apartment");*/
            
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TypeCombo.SelectedIndex != -1)
            {
                switch (TypeCombo.Text)
                {
                    case "Apartment":
                        SelectedType = AccommodationType.Apartment;
                        break;

                    case "House":
                        SelectedType = AccommodationType.House;
                        break;

                    case "Shack":
                        SelectedType = AccommodationType.Shack;
                        break;
                }

            }
            else
            {
                SelectedType = (AccommodationType)TypeCombo.SelectedIndex;
            }
            Accommodations = _accommodationsRepository.FilterAccommodations(SelectedMinReservationDays, SelectedType, SelectedMaxGuest, SelectedName, SelectedLocation.City, SelectedLocation.Country);
            AccommodationsListView.ItemsSource = Accommodations;
        }
        private void CountryComboBox_Selected(object sender, EventArgs e)
        {
            if (CountryComboBox.SelectedIndex != -1)
            {
                CityComboBox.IsEnabled = true;
            //    Cities = new ObservableCollection<string>(_locationRepository.GetLocations()[SelectedLocation.Country]);
                CityComboBox.ItemsSource = Cities;
            }
        }

        /*   private void TypeComboBox_Selected(object sender, SelectionChangedEventArgs e)
           {

               if (TypeComboBox.SelectedItem != null)
               {
                   AccommodationType selectedType = (AccommodationType)TypeComboBox.SelectionBoxItem;
                //   Types = new ObservableCollection<int>(_accommodationsRepository.IsSelectedAccommodationType()[SelectedType.AccommodationType]);
                TypeComboBox.ItemsSource = Types;
               }
           }
           */
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the user is trying to paste something into the textbox
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Get the clipboard data and check if it's a valid number or a dot
                if (!double.TryParse(Clipboard.GetText(), out double result) || result < 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the input is a number or a dot
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }
        }
        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the input is a letter or a space
            if (!char.IsLetter(e.Text, 0) && e.Text != " ")
            {
                e.Handled = true;
            }
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {

            SelectedLocation.Country = "";
            SelectedLocation.City = "";
            SelectedName = null;
            SelectedMaxGuest = null;
            SelectedMinReservationDays = null;
            CountryComboBox.SelectedIndex = -1;
            CityComboBox.SelectedIndex = -1;
            TypeCombo.SelectedIndex = -1;
            NameTextBox.Clear();
            CityComboBox.IsEnabled = false;
            GuestLimitTextBox.Clear();
            MinReservationDaysTextBox.Clear();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationsRepository.GetAll());
            AccommodationsListView.ItemsSource = Accommodations;

        }
        private void AccommodationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ShowAccommodationReservationForm(object sender, RoutedEventArgs e)
        {
            GuestReservationAccommodation createAccommodationReservationForm = new GuestReservationAccommodation();
            createAccommodationReservationForm.Show();
        }

        private void CancelClick_Button(object sender, RoutedEventArgs e)
        {
         this.Close();
        }
    }
}