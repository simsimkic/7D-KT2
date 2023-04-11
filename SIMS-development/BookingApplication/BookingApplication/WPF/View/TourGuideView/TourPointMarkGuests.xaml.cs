using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointMarkGuests.xaml
    /// </summary>
    public partial class TourPointMarkGuests : Window
    {
        public TourPoint SelectedTourPoint { get; set; }


        public static ObservableCollection<TourReservation> NotPresentGuestTourReservations { get; set; }
        public static ObservableCollection<TourReservation> PresentGuestTourReservations { get; set; }

        public TourReservation SelectedTourReservation { get; set; }
        public TourGuestDTO SelectedTourGuestDTO { get; set; }

        private readonly TourReservationRepository _tourReservationRepository;
        private readonly UserRepository _userRepository;

        private readonly TourPointService _tourPointService;

        public static ObservableCollection<TourGuestDTO> NotPresentGuestTourReservationsDTO { get; set; }
        public static ObservableCollection<TourGuestDTO> PresentGuestTourReservationsDTO { get; set; }


        public TourPointMarkGuests(TourPoint selectedTourPoint)
        {
            InitializeComponent();
            DataContext = this;


            _tourReservationRepository = new TourReservationRepository();
            _userRepository = new UserRepository();
            _tourPointService = new TourPointService();

            SelectedTourPoint = selectedTourPoint;
            SelectedTourReservation = new TourReservation();
            SelectedTourGuestDTO = new TourGuestDTO();

            PresentGuestTourReservations = new ObservableCollection<TourReservation>();
            //method for selecting specific tour should be implemented
            NotPresentGuestTourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());

            PresentGuestTourReservationsDTO = new ObservableCollection<TourGuestDTO>();
            NotPresentGuestTourReservationsDTO = new ObservableCollection<TourGuestDTO>();

            AssignReservationsToDTO(NotPresentGuestTourReservations, NotPresentGuestTourReservationsDTO);
        }

        private void AssignReservationsToDTO(ObservableCollection<TourReservation> guestReservations, 
            ObservableCollection<TourGuestDTO> tourGuestReservationsDTO)
        {
            foreach (var guestReservation in guestReservations)
            {
                /*tourId needs to be replaced by tourReservationId*/
                TourGuestDTO guestReservationDTO = new TourGuestDTO(guestReservation.UserId,
                    GetUsername(guestReservation), guestReservation.TourId, guestReservation.GroupSize);
                tourGuestReservationsDTO.Add(guestReservationDTO);
            }
        }

        private String GetUsername(TourReservation guestReservation)
        {
            foreach (var user in _userRepository.GetAll().Where(user => guestReservation.UserId == user.Id))
            {
                return user.Username;
            }
            return null;
        }


        private TourReservation GetTourReservationByGuestDTO(TourGuestDTO tourGuestDTO)
        {
            foreach (var guestReservation in PresentGuestTourReservations)
            {
                /*tourId needs to be replaced by tourReservationId*/
                if (tourGuestDTO.TourReservationId == guestReservation.TourId)
                {
                    return guestReservation;
                }
            }

            foreach (var guestReservation in NotPresentGuestTourReservations)
            {
                if (tourGuestDTO.TourReservationId == guestReservation.TourId)
                {
                    return guestReservation;
                }
            }

            return null;
        }
        private void MarkGuestClick(object sender, RoutedEventArgs e)
        {
            SelectedTourReservation = GetTourReservationByGuestDTO(SelectedTourGuestDTO);

            //SelectedTourReservation.TourPointId = SelectedTourPoint.Id;
            //SelectedTourReservation.ReservationChecked = True;

            TourGuestDTO newSelectedTourGuestDTO = new TourGuestDTO(SelectedTourGuestDTO.UserId,
                SelectedTourGuestDTO.UserName, SelectedTourGuestDTO.TourReservationId, 
                SelectedTourGuestDTO.TourReservationGroupSize);

            NotPresentGuestTourReservationsDTO.Remove(SelectedTourGuestDTO);
            NotPresentGuestTourReservations.Remove(SelectedTourReservation);


            PresentGuestTourReservationsDTO.Add(newSelectedTourGuestDTO);
            PresentGuestTourReservations.Add(SelectedTourReservation);
        }

        private void UnmarkGuestClick(object sender, RoutedEventArgs e)
        {
            SelectedTourReservation = GetTourReservationByGuestDTO(SelectedTourGuestDTO);

            //SelectedTourReservation.TourPointId = -1;
            //SelectedTourReservation.ReservationChecked = False;

            PresentGuestTourReservationsDTO.Remove(SelectedTourGuestDTO);
            PresentGuestTourReservations.Remove(SelectedTourReservation);

            NotPresentGuestTourReservationsDTO.Add(SelectedTourGuestDTO);
            NotPresentGuestTourReservations.Add(SelectedTourReservation);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SelectedTourPoint.Active = true;
            _tourPointService.UpdateTourPoint(SelectedTourPoint);
            SaveTourReservations(PresentGuestTourReservations);
            SaveTourReservations(NotPresentGuestTourReservations);
            Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveTourReservations(ObservableCollection<TourReservation> tourReservations)
        {
            foreach (var tourReservation in tourReservations)
            {
                _tourReservationRepository.Update(tourReservation);
            }
        }
    }
}
