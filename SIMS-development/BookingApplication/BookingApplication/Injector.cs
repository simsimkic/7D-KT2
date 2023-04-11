﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Repository;

namespace BookingApplication
{

    public class Injector
        {
            private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
            {
                { typeof(IUserRepository), new UserRepository() },
                { typeof(ICommentRepositoy), new CommentRepository() },
                { typeof(IAccommodationRepository), new AccommodationRepository() },
                { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
                { typeof(IGuestRepository), new GuestRepository() },
                { typeof(IGuestRatingRepository), new GuestRatingRepository() },
                { typeof(IImageRepository), new ImageRepository() },
                { typeof(ILocationRepository), new LocationRepository() },
                { typeof(ITourDateRepository), new TourDateRepository() },
                { typeof(ITourPointRepository), new TourPointRepository() },
                { typeof(ITourRepository), new TourRepository() },
                { typeof(ITourReservationRepository), new TourReservationRepository() },
                { typeof(IVoucherRepository), new VoucherRepository() }

                // Add more implementations here
            };

            public static T CreateInstance<T>()
            {
                Type type = typeof(T);

                if (_implementations.ContainsKey(type))
                {
                    return (T)_implementations[type];
                }

                throw new ArgumentException($"No implementation found for type {type}");
            }
        }
    
}
