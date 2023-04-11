using System;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourDateForm.xaml
    /// </summary>
    public partial class TourDateForm : Window
    {

        public TourDate SelectedTourDate { get; set; }

        private readonly  TourDateService _tourDateService;
        private bool _isNew;
        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            set
            {
                if (_isNew != value)
                {
                    _isNew = value;
                }
            }
        }

        private int _hours;
        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                if (value != _hours)
                {
                    _hours = value;
                }
            }
        }

        private int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (value != _minutes)
                {
                    _minutes = value;
                }
            }
        }

        private DateTime _inputDate;

        public DateTime InputDate
        {
            get
            {
                return _inputDate;
            }
            set
            {
                if (value != _inputDate)
                {
                    _inputDate=value;
                }
            }
        }

        

        public TourDateForm(Tour selectedTour, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Add Tour Date";

            _tourDateService = new TourDateService();
            SelectedTourDate = new TourDate(selectedTour.Id, DateTime.Now);
            InputDate = DateTime.Now;
        }

        public TourDateForm(Tour selectedTour, TourDate selectedTourDate, bool isNew)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Update Tour Date";

            _tourDateService = new TourDateService();
            SelectedTourDate = selectedTourDate;
            InputDate = SelectedTourDate.StartTime;
        }

        private void SaveTourDateClick(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                TourDateParse();
                _tourDateService.SaveTourDate(SelectedTourDate);
                TourForm.TourDates.Add(SelectedTourDate);
                Close();
            }

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool AreComboBoxesSelected()
        {
            if (MiddayComboBox.SelectionBoxItem != null && HourComboBox.SelectionBoxItem != null &&
                MinuteComboBox.SelectionBoxItem != null)
                return true;
            return false;
        }

        private bool DetermineMidday()
        {
            if (MiddayComboBox.SelectedValue.ToString() == "PM")
            {
                return true;
            }
            return false;
        }

        private void TourDateParse()
        {
            if (DetermineMidday())
            {
                Hours = 12;
            }

            //Hours = Hours + Convert.ToInt32(HourComboBox.SelectedValue.ToString());
            //Minutes = Minutes + Convert.ToInt32(MinuteComboBox.SelectedValue.ToString());

            String hourString = HourComboBox.SelectionBoxItem.ToString();
            String minuteString = MinuteComboBox.SelectionBoxItem.ToString();
            
            Minutes = Convert.ToInt32(minuteString);
            Hours += Convert.ToInt32(hourString);
            

            SelectedTourDate.StartTime =
                new DateTime(InputDate.Year, InputDate.Month, InputDate.Day, Hours, Minutes, 0);

        }

        
    }
}



