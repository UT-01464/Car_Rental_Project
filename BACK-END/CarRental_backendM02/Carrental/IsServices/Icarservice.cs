//using Carrental.Dtos.RequestDTO;
//using Carrental.Dtos.ResponceDTO;
//using System.Threading.Tasks;


//namespace Carrental.IsServices
//{
//    public interface Icarservice
//    {

//        Task<carresponseDTO> AddCarAsync(carrequestDTO carrequestDTO);
//        Task<carresponseDTO> GetCarByIdAsync(Guid carId);
//        Task<carresponseDTO> EditCar(Guid carid, carrequestDTO carRequest);
//        Task<bool> DeleteCarAsync(Guid carId);
//        Task<List<carresponseDTO>> GetAllCars();


//    }
//}
using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponseDTO; // Ensure to use the correct namespace for CarResponseDTO
using Microsoft.AspNetCore.Http; // Include this for IFormFile
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carrental.IServices
{
    public interface ICarService
    {
        Task<CarResponseDTO> AddCarAsync(CarRequestDTO carRequestDTO, IFormFile imageFile);
        Task<CarResponseDTO> GetCarByIdAsync(Guid carId);
        Task<CarResponseDTO> EditCar(Guid carId, CarRequestDTO carRequest, IFormFile imageFile);
        Task<string> DeleteCarAsync(Guid carId);
        Task<List<CarResponseDTO>> GetAllCars();
    }
}
