using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;

namespace Carrental.IsServices
{
    public interface IManagerService
    {
        Task<List<RentalResponseDTO>> GetAllRentals();
        Task<RentalResponseDTO> GetRentalById(Guid id);
        Task<List<RentalResponseDTO>> GetAllRentalsByCustomerId(Guid customerId);

        Task<RentalResponseDTO> AddRental(RentalRequestDTO rentalRequestDTO);
        Task<RentalResponseDTO> AcceptRental(Guid id);
        Task<bool> RejectRental(Guid rentalid);
        Task<RentalResponseDTO> UpdateReturn(Guid id);
        Task<List<Guid>> CheckAndUpdateOverdueRentals();
    }
}
