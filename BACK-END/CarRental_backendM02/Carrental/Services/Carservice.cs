//using Carrental.Dtos.RequestDTO;
//using Carrental.Dtos.ResponceDTO;
//using Carrental.Entities;
//using Carrental.IRepositries;
//using Carrental.IsServices;
//using Carrental.Repositoies;
//using Microsoft.AspNetCore.Hosting;

//namespace Carrental.Services
//{
//    public class Carservice : Icarservice
//    {
//        private readonly Icarrepository _carrepository;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        public Carservice( Icarrepository icarrepository, IWebHostEnvironment webHostEnvironment)
//        {
//            _carrepository = icarrepository;
//            _webHostEnvironment = webHostEnvironment;
//        }

//        public async Task<carresponseDTO> AddCarAsync(carrequestDTO carrequestDTO)
//        {
//            if (carrequestDTO == null)
//            {
//                throw new ArgumentNullException(nameof(carrequestDTO), "Car request cannot be null.");
//            }

//            // Create a new Car object from the request DTO
//            var car = new Car
//            {
//                Id = Guid.NewGuid(), // Generating a new ID for the car
//                Title = carrequestDTO.Title,
//                RegistorNo = carrequestDTO.Regnumber,
//                Brand = carrequestDTO.Brand,
//                Description = carrequestDTO.Description,
//                Model = carrequestDTO.Model,
//                IsAvailable = true // Assuming cars are available when added
//            };

//            // Handle the image upload if the image file is provided
//            if (carrequestDTO.ImageFile != null && carrequestDTO.ImageFile.Length > 0)
//            {
//                car = await _carrepository.AddCar(car, carrequestDTO.ImageFile); // Delegate image handling to the repository
//            }
//            else
//            {
//                // If no image, still add the car without the image
//                car = await _carrepository.AddCar(car, null);
//            }

//            // Create and return the response DTO with the car details
//            return new carresponseDTO
//            {
//                Title = car.Title,
//                ImageUrl = car.CarImage,
//                Regnumber = car.RegistorNo,
//                Brand = car.Brand,
//                Description = car.Description,
//                Model = car.Model,
//                IsAvailable = car.IsAvailable
//            };
//        }
//        public async Task<carresponseDTO> GetCarByIdAsync(Guid carId)
//        {
//            var car = await _carrepository.GetCarById(carId);
//            if (car == null) return null;

//            return new carresponseDTO
//            {
//                ID = car.Id,
//                Title = car.Title,
//                ImageUrl = car.CarImage,
//                Regnumber = car.RegistorNo,
//                Brand = car.Brand,
//                Description = car.Description,
//                Model = car.Model,
//                IsAvailable = car.IsAvailable,
//            };
//        }

//        public async Task<List<carresponseDTO>> GetAllCars()
//        {

//            var cars = await _carrepository.GetAllCars();


//            return cars.Select(c => new carresponseDTO
//            {
//                ID = c.Id,
//                Title = c.Title,
//                ImageUrl = c.CarImage,
//                Regnumber = c.RegistorNo,
//                Brand = c.Brand,

//                Description = c.Description,
//                Model = c.Model,
//                IsAvailable = c.IsAvailable,
//            }).ToList();
//        }

//        public async Task<carresponseDTO> EditCar(Guid carid, carrequestDTO carRequest)
//        {

//            var car = await _carrepository.GetCarById(carid);
//            if (car == null) return null;


//            car.Title = carRequest.Title;
//            car.RegistorNo = carRequest.Regnumber;
//            car.Brand = carRequest.Brand;
//            car.Description = carRequest.Description;
//            car.Model = carRequest.Model;

//            if (carRequest.ImageFile != null && carRequest.ImageFile.Length > 0)
//            {

//                if (!string.IsNullOrEmpty(car.CarImage))
//                {
//                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, car.CarImage.TrimStart('/'));
//                    if (File.Exists(oldImagePath))
//                    {
//                        File.Delete(oldImagePath);
//                    }
//                }


//                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(carRequest.ImageFile.FileName);
//                var newImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "carimages", fileName);

//                using (var stream = new FileStream(newImagePath, FileMode.Create))
//                {
//                    await carRequest.ImageFile.CopyToAsync(stream);
//                }

//                car.CarImage = "/carimages/" + fileName;
//            }

//            await _carrepository.EditCar(car);


//            return new carresponseDTO
//            {
//                ID = car.Id,
//                Title = car.Title,
//                ImageUrl = car.CarImage,
//                Regnumber = car.RegistorNo,
//                Brand = car.Brand,
//                Description = car.Description,
//                Model = car.Model,
//                IsAvailable = car.IsAvailable,
//            };
//        }

//        public async Task<bool> DeleteCarAsync(Guid carId)
//        {

//            var car = await _carrepository.GetCarById(carId);
//            if (car == null) return false;

//            if (!string.IsNullOrEmpty(car.CarImage))
//            {
//                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, car.CarImage.TrimStart('/'));
//                if (File.Exists(fullPath))
//                {
//                    File.Delete(fullPath);
//                }
//            }

//            await _carrepository.DeleteCar(carId);
//            return true;
//        }



//    }
//}

using Carrental.Dtos.RequestDTO; // Ensure the correct namespace for CarRequestDTO
using Carrental.Dtos.ResponseDTO; // Ensure the correct namespace for CarResponseDTO
using Carrental.IRepositories; // Ensure the correct namespace for ICarRepository
using Carrental.Helpers; // Ensure the correct namespace for ImageUploadHelper
using Carrental.Entities; // Ensure the correct namespace for Car entity
using Microsoft.AspNetCore.Http; // For IFormFile
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carrental.IServices;

namespace Carrental.Services
{
    public class CarService : ICarService // Make sure to implement ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ImageUploadHelper _imageUploadHelper;

        // Constructor
        public CarService(ICarRepository carRepository, ImageUploadHelper imageUploadHelper)
        {
            _carRepository = carRepository;
            _imageUploadHelper = imageUploadHelper; // Initialize the image upload helper
        }

        public async Task<CarResponseDTO> AddCarAsync(CarRequestDTO carRequestDTO, IFormFile imageFile)
        {
            // Validate the input DTO
            if (carRequestDTO == null || imageFile == null)
            {
                throw new ArgumentException("Invalid car request or image file.");
            }

            // Upload the image and get the URL
            string imageUrl = await _imageUploadHelper.UploadImageAsync(imageFile);

            // If image upload fails, handle it (optional)
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new InvalidOperationException("Image upload failed.");
            }

            // Create a Car object from the DTO
            var car = new Car
            {
                Id = Guid.NewGuid(), // Generate a new GUID for the Id
                Title = carRequestDTO.Title,
                CarImage = imageUrl, // Set the image URL
                RegistorNo = carRequestDTO.RegistorNo,
                Brand = carRequestDTO.Brand,
                Description = carRequestDTO.Description,
                Model = carRequestDTO.Model,
                RentalPrice=carRequestDTO.RentalPrice,
                Category=carRequestDTO.Category,
                IsAvailable = true // Assuming a new car is available
            };

            // Call the repository method to add the car to the database
            await _carRepository.AddCarAsync(car,imageFile); // Pass only the car object, imageFile is no longer needed

            // Return a response DTO
            return new CarResponseDTO
            {
                ID = car.Id,
                Title = car.Title,
                RegistorNo = car.RegistorNo,
                Brand = car.Brand,
                Description = car.Description,
                Model = car.Model,
                RentalPrice=car.RentalPrice,
                Category=car.Category,
                ImageUrl = car.CarImage, // Set the ImageUrl from the CarImage property
                IsAvailable = car.IsAvailable
            };
        }


        public async Task<CarResponseDTO> GetCarByIdAsync(Guid carId)
        {
            return await _carRepository.GetCarByIdAsync(carId);
        }

        public async Task<CarResponseDTO> EditCar(Guid carId, CarRequestDTO carRequest, IFormFile imageFile)
        {
            // Validate the input
            if (carRequest == null)
            {
                throw new ArgumentException("Invalid car request.");
            }

            // Upload the image if provided
            string imageUrl = null;
            if (imageFile != null)
            {
                imageUrl = await _imageUploadHelper.UploadImageAsync(imageFile);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new InvalidOperationException("Image upload failed.");
                }
            }

            // Call the repository method to edit the car in the database
            return await _carRepository.EditCar(carId, carRequest, imageUrl);
        }

        public async Task<string> DeleteCarAsync(Guid carId)
        {
            return await _carRepository.DeleteCarAsync(carId);
        }

        public async Task<List<CarResponseDTO>> GetAllCars()
        {
            return await _carRepository.GetAllCars();
        }
    }
}

