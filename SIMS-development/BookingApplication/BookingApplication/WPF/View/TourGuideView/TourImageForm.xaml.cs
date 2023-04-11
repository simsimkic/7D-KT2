using System;
using System.Windows;
using Image = BookingApplication.Domain.Model.Image;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourImageForm.xaml
    /// </summary>
    public partial class TourImageForm : Window
    {
        private readonly TourRepository _tourRepository;
        private readonly ImageService _imageService;
        public Tour SelectedTour { get; set; }
        public Image SelectedImage { get; set; }
        public TourImageForm(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Add Image";
            _tourRepository = new TourRepository();
            _imageService = new ImageService();

            SelectedTour = selectedTour;
            SelectedImage = new Image();

        }

        private void SaveImageClick(object sender, EventArgs e)
        {
            if (true)
            {
                ConfigureSelectedImage();
                SelectedTour.ImageId.Add(SelectedImage.Id);
                TourImagesOverview.Images.Add(SelectedImage);
                Close();
            }

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
          
        }


    }
}
