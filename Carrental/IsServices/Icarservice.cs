using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;

namespace Carrental.IsServices
{
    public interface Icarservice
    {

        Task<carresponseDTO> AddcarAsync(carrequestDTO carrequestDTO);
        Task<carresponseDTO> GetCarByIdAsync(Guid carId);
        Task<carresponseDTO> EditCar(Guid carid, carrequestDTO carRequest);
        Task<bool> DeleteCarAsync(Guid carId);
        Task<List<carresponseDTO>> GetAllCars();


    }
}
