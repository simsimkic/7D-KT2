using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Repository;
using BookingApplication.Serializer;


namespace BookingApplication.Domain.Model
{

    public class Tour : ISerializable, INotifyPropertyChanged
    {

        private int _id;
        private string _name;
        private int _locationId;
        private string _description;
        private string _language;
        private int _maxGuests;
        private List<TourDate> _possibleDates;
        private double _duration;
        private List<TourPoint> _tourPoints;
        private bool _began;
        private List<int> _imageId;
        private string _thumbnailUrl;
        private int _guideId;
        public Tour()
        {

        }

        public Tour(string name, int locationId, string description, string language, int maxGuests,
            List<TourDate> possibleDates, double duration, bool began, List<int> imageId, string thumbnailUrl,
            int guideId)
        {
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            PossibleDates = new List<TourDate>(possibleDates);
            Duration = duration;
            TourPoints = new List<TourPoint>();
            Began = began;
            ImageId = new List<int>(imageId);
            ThumbnailUrl = thumbnailUrl;
            GuideId = guideId;
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (_maxGuests != value)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<TourDate> PossibleDates
        {
            get => _possibleDates;
            set
            {
                if (_possibleDates != value)
                {
                    _possibleDates = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<TourPoint> TourPoints
        {
            get => _tourPoints;
            set
            {
                if (_tourPoints != value)
                {
                    _tourPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Began
        {
            get => _began;
            set
            {
                if (_began != value)
                {
                    _began = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<int> ImageId
        {
            get => _imageId;
            set
            {
                if (_imageId != value)
                {
                    _imageId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set
            {
                if (value != _thumbnailUrl)
                {
                    _thumbnailUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuideId
        {
            get => _guideId;
            set
            {
                if (value != _guideId)
                {
                    _guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), Name, LocationId.ToString(), Description, Language, MaxGuests.ToString(),
                DatesToString(PossibleDates), Duration.ToString(), Began.ToString(), ImagesToString(ImageId), ThumbnailUrl,
                GuideId.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuests = Convert.ToInt32(values[5]);
            PossibleDates = GetDatesFromIds(GetDatesIds(values[6]));
            Duration = Convert.ToDouble(values[7]);
            Began = Convert.ToBoolean(values[8]);
            ImageId = GetImages(values[9]);
            ThumbnailUrl = values[10];
            GuideId = Convert.ToInt32(values[11]);
        }

        private string DatesToString(List<TourDate> dates)
        {
            string datesString = "";
            foreach (var date in dates)
            {
                if (date != dates.Last())
                {
                    datesString += date.Id + ",";
                    continue;
                }
                datesString += date.Id;
            }

            return datesString;
        }

        private List<int> GetDatesIds(string dateString)
        {
            List<int> dates = new List<int>();
            string[] dateStrings = dateString.Split(',');

            foreach (string dateStr in dateStrings)
            {
                int.TryParse(dateStr, out var date);
                dates.Add(date);
            }
            return dates;
        }

        private List<TourDate> GetDatesFromIds(List<int> ids)
        {
            TourDateRepository tourDateRepository = new TourDateRepository();
            List<TourDate> dates = new List<TourDate>();
            foreach (var id in ids)
            {
                dates.Add(tourDateRepository.GetById(id));
            }

            return dates;
        }

        private string ImagesToString(List<int> images)
        {
            string imagesString = "";
            foreach (var image in images)
            {
                if (image != images.Last())
                {
                    imagesString += image + ",";
                    continue;
                }
                imagesString += image;
            }

            return imagesString;
        }


        private List<int> GetImages(string imagesString)
        {
            List<int> images = new List<int>();
            string[] ids = imagesString.Split(',');
            foreach (var iterator in ids)
            {
                int.TryParse(iterator, out var id);
                images.Add(id);
            }

            return images;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
            set
            {
                value = true;
            }

        }
    }
}