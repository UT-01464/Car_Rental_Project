using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface IManagerRepository
    {
        Task<List<Rental>> GetAllRentals();
        Task<Rental> GetRentalByID(Guid rentalId);
        Task<List<Rental>> GetAllRentalsByCustomerID(Guid customerId);
        Task<Rental> AddRental(Rental rental);
        Task<Rental> AcceptRental(Rental rental);
        Task<Rental> RejectRental(Guid id);
        Task<Rental> UpdateReturn(Rental rental);
        Task<List<Guid>> CheckAndUpdateOverdueRentals();
    }
}
