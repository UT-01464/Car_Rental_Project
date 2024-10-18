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
                    id = item.Id,
                    CCarId = item.CarId,
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
                id = data.Id,
                CustomerId = data.CustomerId,
                CCarId = data.CarId,
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
                    id = data.Id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CarId,
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
                CarId = rentalRequestDTO.CCarId,
                RentalDate = rentalRequestDTO.RentalDate,
                ReturnDate = rentalRequestDTO.ReturnDate,
            };

            var data = await _Repository.AddRental(rental);

            var rentalresp = new RentalResponseDTO
            {
                id = data.Id,
                CustomerId = data.CustomerId,
                CCarId = data.CarId,
                ReturnDate = data.ReturnDate,
                Status = data.Status,
                OverDue = data.OverDue,
                RentalDate = data.RentalDate,
            };

            return rentalresp;
        }

        //public async Task<RentalResponseDTO> AcceptRental(Guid id)
        //{
        //    var Rentdata = await _Repository.GetRentalByID(id);
        //    if (Rentdata.Status == "Pending")
        //    {
        //        var data = await _Repository.AcceptRental(Rentdata);

        //        var RentalRespon = new RentalResponseDTO
        //        {
        //            id = data.Id,
        //            CustomerId = data.CustomerId,
        //            CCarId = data.CarId,
        //            ReturnDate = data.ReturnDate,
        //            Status = data.Status,
        //            OverDue = data.OverDue,
        //            RentalDate = data.RentalDate,
        //        };

        //        return RentalRespon;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public async Task<RentalResponseDTO> AcceptRental(Guid id)
        {
            var rentalResponse = new RentalResponseDTO(); // Initialize the response DTO

            try
            {
                // Fetch the rental data by ID
                var rentData = await _Repository.GetRentalByID(id);

                // Check if rentData is null
                if (rentData == null)
                {
                    rentalResponse.Status = "Not Found"; // Or throw a custom exception
                    return rentalResponse;
                }

                // Check the status of the rental
                if (rentData.Status == "Pending")
                {
                    // Accept the rental
                    var updatedRental = await _Repository.AcceptRental(rentData.Id); // Use rentData.Id

                    // Create the response DTO
                    rentalResponse = new RentalResponseDTO
                    {
                        id = updatedRental.Id,
                        CustomerId = updatedRental.CustomerId,
                        CCarId = updatedRental.CarId,
                        ReturnDate = updatedRental.ReturnDate,
                        Status = updatedRental.Status,
                        OverDue = updatedRental.OverDue,
                        RentalDate = updatedRental.RentalDate,
                    };

                    return rentalResponse;
                }
                else
                {
                    // Return a response indicating the rental cannot be accepted
                    rentalResponse.Status = "Rental cannot be accepted, current status: " + rentData.Status;
                    return rentalResponse;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.Error.WriteLine($"Error accepting rental: {ex.Message}"); // Or use a logging framework

                // Return a generic error response
                rentalResponse.Status = "Error occurred while accepting rental.";
                return rentalResponse;
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
                    id = data.Id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CarId,
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
