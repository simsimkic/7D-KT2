using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Services
{
    class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly TourReservationService _tourReservationService;

        public VoucherService()
        {
            _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        }

        public void DeleteVoucher(Voucher voucher)
        {
            _voucherRepository.Delete(voucher);
        }

        public void SaveVoucher(Voucher voucher)
        {
            _voucherRepository.Save(voucher);
        }

        public Voucher UpdateVoucher(Voucher voucher)
        {
            return _voucherRepository.Update(voucher);
        }

        public List<Voucher> GetAllVouchers()
        {
            return _voucherRepository.GetAll();
        }

        public void AssignVouchersToToursGuests(Tour tour)
        {
            foreach (TourReservation tourReservation in _tourReservationService.GetAllTourReservations())
            {
                if (tourReservation.TourId == tour.Id)
                {

                    Voucher voucher = new Voucher(tourReservation.UserId, tour.GuideId);
                    SaveVoucher(voucher);
                }
            }
        }

    }
}
