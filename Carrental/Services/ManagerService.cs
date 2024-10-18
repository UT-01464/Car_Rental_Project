using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Entities;
using Carrental.IRepositries;
using Carrental.IsServices;
using Carrental.Repositoies;

namespace Carrental.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _Repository;

        public ManagerService(IManagerRepository repository)
        {

            _Repository = repository;
        }

        public async Task<List<RentalResponseDTO>> GetAllRentals()
        {
            var customer = await _Repository.GetAllRentals();

            var data = new List<RentalResponseDTO>();
            foreach (var item in customer)
            {
                var rentalresponse = new RentalResponseDTO
                {
                    id = item.id,
                    CCarId = item.CCarId,
                    CustomerId = item.CustomerId,
                    RentalDate = item.RentalDate,
                    ReturnDate = item.ReturnDate,
                    OverDue = item.OverDue,
                    Status = item.Status,
                };

                data.Add(rentalresponse);
            }

            return data;
        }

        public async Task<RentalResponseDTO> GetRentalById(Guid id)
        {
            var data = await _Repository.GetRentalByID(id);
            var rentalresp = new RentalResponseDTO
            {
                id = data.id,
                CustomerId = data.CustomerId,
                CCarId = data.CCarId,
                ReturnDate = data.ReturnDate,
                Status = data.Status,
                OverDue = data.OverDue,
                RentalDate = data.RentalDate,
            };

            return rentalresp;
        }

        public async Task<List<RentalResponseDTO>> GetAllRentalsByCustomerId(Guid customerId)
        {
            var rentals = await _Repository.GetAllRentalsByCustomerID(customerId);

            var rentaldata = new List<RentalResponseDTO>();

            foreach (var data in rentals)
            {
                var rentalresp = new RentalResponseDTO
                {
                    id = data.id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CCarId,
                    ReturnDate = data.ReturnDate,
                    Status = data.Status,
                    OverDue = data.OverDue,
                    RentalDate = data.RentalDate,
                };

                rentaldata.Add(rentalresp);
            }
            return rentaldata;
        }


        public async Task<RentalResponseDTO> AddRental(RentalRequestDTO rentalRequestDTO)
        {
            var rental = new Rental
            {
                CustomerId = rentalRequestDTO.CustomerId,
                CCarId = rentalRequestDTO.CCarId,
                RentalDate = rentalRequestDTO.RentalDate,
                ReturnDate = rentalRequestDTO.ReturnDate,
            };

            var data = await _Repository.AddRental(rental);

            var rentalresp = new RentalResponseDTO
            {
                id = data.id,
                CustomerId = data.CustomerId,
                CCarId = data.CCarId,
                ReturnDate = data.ReturnDate,
                Status = data.Status,
                OverDue = data.OverDue,
                RentalDate = data.RentalDate,
            };

            return rentalresp;
        }

        public async Task<RentalResponseDTO> AcceptRental(Guid id)
        {
            var Rentdata = await _Repository.GetRentalByID(id);
            if (Rentdata.Status == "Pending")
            {
                var data = await _Repository.AcceptRental(Rentdata);

                var RentalRespon = new RentalResponseDTO
                {
                    id = data.id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CCarId,
                    ReturnDate = data.ReturnDate,
                    Status = data.Status,
                    OverDue = data.OverDue,
                    RentalDate = data.RentalDate,
                };

                return RentalRespon;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> RejectRental(Guid rentalid)
        {
            var rental = await _Repository.GetRentalByID(rentalid);
            if (rental == null) return false;

            await _Repository.RejectRental(rentalid);
            return true;
        }


        public async Task<RentalResponseDTO> UpdateReturn(Guid id)
        {
            var Rentdata = await _Repository.GetRentalByID(id);
            if (Rentdata.Status == "Rent")
            {
                var data = await _Repository.UpdateReturn(Rentdata);

                var RentalRespon = new RentalResponseDTO
                {
                    id = data.id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CCarId,
                    Status = data.Status,
                    OverDue = data.OverDue,
                    RentalDate = data.RentalDate,
                    ReturnDate = data.ReturnDate,
                };

                return RentalRespon;
            }
            else
            {
                return null;
            }
        }


        public async Task<List<Guid>> CheckAndUpdateOverdueRentals()
        {
            var overdue = await _Repository.CheckAndUpdateOverdueRentals();
            if (overdue == null) return null;
            return overdue;
        }
    }
}
