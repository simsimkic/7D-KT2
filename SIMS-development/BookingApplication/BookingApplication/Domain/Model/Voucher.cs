using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class Voucher : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _guestId;
        private int _guideId;

        public Voucher() { }

        public Voucher(int guestId, int guideId)
        {
            GuestId = guestId;
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

        public int GuestId
        {
            get => _guestId;
            set
            {
                if (_guestId != value)
                {
                    _guestId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuideId
        {
            get => _guideId;
            set
            {
                if (_guideId != value)
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

                Id.ToString(), GuestId.ToString(), GuideId.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
