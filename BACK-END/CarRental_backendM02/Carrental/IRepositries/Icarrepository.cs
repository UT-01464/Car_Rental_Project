using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Dtos.ResponseDTO;
using Carrental.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carrental.IRepositories
{
    public interface ICarRepository
    {
        Task<Car> AddCarAsync(Car car, IFormFile imageFile);
        Task<CarResponseDTO> GetCarByIdAsync(Guid carId);
        Task<CarResponseDTO> EditCar(Guid carId, CarRequestDTO carRequest, string imageUrl);
        Task<string> DeleteCarAsync(Guid carId);
        Task<List<CarResponseDTO>> GetAllCars();
    }
}
