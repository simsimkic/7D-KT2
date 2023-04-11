
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
    public class Location : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        private int _id;
        private string _city;
        private string _country;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        public Location() { }



        public static bool ContainsNumber(string str)
        {
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Country")
                {
                    if (string.IsNullOrEmpty(Country) || !char.IsLetter(Country[0]) || ContainsNumber(Country))
                        return "It's not in a correct form!";
                }


                else if (columnName == "City")
                {
                    if (string.IsNullOrEmpty(City) || !char.IsLetter(City[0]) || ContainsNumber(City))
                        return "It's not in a correct form!";
                }



                return null;
            }
        }

        private readonly string[] validatedProperties = { "Country", "City" };

        public bool IsValid
        {
            get
            {
                foreach (var property in validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }


        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), City, Country
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            City = values[1];
            Country = values[2];
        }

        public override string ToString()
        {
            return Country + ", " + City;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}