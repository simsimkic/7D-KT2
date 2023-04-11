using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class TourReservation : ISerializable, INotifyPropertyChanged
    {

        private int _userId;
        private int _tourId;
        private int _groupSize;
        private DateTime _date;

        public TourReservation() { }
        public TourReservation(int userId, int tourId, int groupSize, DateTime date)
        {
            UserId = userId;
            TourId = tourId;
            GroupSize = groupSize;
            Date = date;
        }

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TourId
        {
            get => _tourId;
            set
            {
                if (_tourId != value)
                {
                    _tourId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GroupSize
        {
            get => _groupSize;
            set
            {
                if (_groupSize != value)
                {
                    _groupSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                UserId.ToString(), TourId.ToString(), GroupSize.ToString(), Date.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            UserId = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            GroupSize = int.Parse(values[2]);
            Date = DateTime.Parse(values[3]);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
