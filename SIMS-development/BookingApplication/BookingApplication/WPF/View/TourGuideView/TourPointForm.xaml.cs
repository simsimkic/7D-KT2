using System;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointForm.xaml
    /// </summary>
    public partial class TourPointForm : Window
    {
        private readonly TourRepository _tourRepository;
        private readonly TourPointRepository _tourPointRepository;
        private readonly ImageService _imageService;
        public Tour SelectedTour { get; set; }

        public TourPoint SelectedTourPoint { get; set; }
        public Image SelectedImage { get; set; }
        
        public enum FormStatus { UPDATE, ADD, LIVE }
        public FormStatus Status { get; set; }

        public int UserId { get; set; }


        public TourPointForm(Tour selectedTour)
        {
            InitializeComponent();  
            DataContext = this;
            Title = "Add Tour";
            _tourRepository = new TourRepository();
            _imageService = new ImageService();
            _tourPointRepository = new TourPointRepository();

            SelectedTour = selectedTour;
            SelectedTourPoint = new TourPoint();
            SelectedImage = new Image();

            Status = FormStatus.ADD;
        }

        public TourPointForm(Tour selectedTour, TourPoint selectedTourPoint)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Update Tour";
            _tourRepository = new TourRepository();
            _tourPointRepository = new TourPointRepository();
            _imageService = new ImageService();

            SelectedTour = selectedTour;
            SelectedTourPoint = selectedTourPoint;

            InitializeSelectedImage();

            Status = FormStatus.UPDATE;
        }

        private void SaveTourPointClick(object sender, EventArgs e)
        {
            if (true)
            {
                ConfigureSelectedImage();

                if (Status == FormStatus.ADD)
                {

                    TourPoint newTourPoint = new TourPoint(SelectedTourPoint.Name, SelectedTour.Id,
                        GuideTourPointOverview.TourPoints.Count+1,
                        SelectedTourPoint.Active, SelectedTourPoint.ImageId);
                    newTourPoint = _tourPointRepository.Save(newTourPoint);
                    GuideTourPointOverview.TourPoints.Add(newTourPoint);
                    Close();
                }
                else if (Status == FormStatus.UPDATE)
                {
                    TourPoint updatedTourPoint = _tourPointRepository.Update(SelectedTourPoint);
                    Image updatedImage = _imageService.Update(SelectedImage);

                    if (updatedTourPoint != null)
                    {
                        int index = GuideTourPointOverview.TourPoints.IndexOf(SelectedTourPoint);
                        GuideTourPointOverview.TourPoints.Remove(SelectedTourPoint);
                        GuideTourPointOverview.TourPoints.Insert(index, updatedTourPoint);
                        Close();
                    }
                }
            }

        }

        private void ConfigureSelectedImage()
        {
            //checking if we already have this image stored in data
            if (_imageService.IsAlreadyInserted(SelectedImage))
            {
                SelectedImage.Id = _imageService.GetIdByUrl(SelectedImage.Url);
            }
            else
            {
                SelectedImage = _imageService.Save(SelectedImage);
            }

            //SelectedTourPoint.ImageId = SelectedImage.Id;
            //SelectedTour.ImageId.Add(SelectedImage.Id);
        
        }



        private void CancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void DisplayImageClick(object sender, RoutedEventArgs e)
        {
            if (urlTxt.Text != null)
            {
                SelectedImage.Url = urlTxt.Text;
            }
        }

        private void InitializeSelectedImage()
        {
            Image initialImage = _imageService.GetById(SelectedTourPoint.ImageId);
            SelectedImage = new Image(initialImage.Url);
        }
        
    }
}
