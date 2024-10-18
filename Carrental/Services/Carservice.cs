using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Entities;
using Carrental.IRepositries;
using Carrental.IsServices;
using Microsoft.AspNetCore.Hosting;

namespace Carrental.Services
{
    public class Carservice : Icarservice
    {
        private readonly Icarrepository _carrepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Carservice( Icarrepository icarrepository, IWebHostEnvironment webHostEnvironment)
        {
            _carrepository = icarrepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<carresponseDTO> AddcarAsync(carrequestDTO carrequestDTO)
        {
            var car = new Car()
            {
                Id = Guid.NewGuid(),
                Title = carrequestDTO.Title,
                RegistorNo = carrequestDTO.Regnumber,
                Brand = carrequestDTO.Brand,
                Description = carrequestDTO.Description,
                Model = carrequestDTO.Model,
            };

            if (carrequestDTO.ImageFile != null && carrequestDTO.ImageFile.Length > 0)
            {

                if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
                {
                    throw new ArgumentNullException(nameof(_webHostEnvironment.WebRootPath), "WebRootPath is not set. Make sure the environment is configured properly.");
                }

                var carimagepath = Path.Combine(_webHostEnvironment.WebRootPath, "carimagepath");

                if (!Directory.Exists(carimagepath))
                {
                    Directory.CreateDirectory(carimagepath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(carrequestDTO.ImageFile.FileName);
                var imagePath = Path.Combine(carimagepath, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await carrequestDTO.ImageFile.CopyToAsync(stream);
                }

               car.CarImage = "/bikeimages/" + fileName;
            }

            var addedcar = await _carrepository.Addcar(car);

            return new carresponseDTO
            {
              
                Title = addedcar.Title,
                ImageUrl = addedcar.CarImage,
                Regnumber = addedcar.RegistorNo,
                Brand = addedcar.Brand,
                Description = addedcar.Description,
                Model = addedcar.Model,
                IsAvailable = addedcar.IsAvailable,
            };
        }
        public async Task<carresponseDTO> GetCarByIdAsync(Guid carId)
        {
            var car = await _carrepository.GetCarById(carId);
            if (car == null) return null;

            return new carresponseDTO
            {
                ID = car.Id,
                Title = car.Title,
                ImageUrl = car.CarImage,
                Regnumber = car.RegistorNo,
                Brand = car.Brand,
                Description = car.Description,
                Model = car.Model,
                IsAvailable = car.IsAvailable,
            };
        }

        public async Task<List<carresponseDTO>> GetAllCars()
        {

            var cars = await _carrepository.GetAllCars();


            return cars.Select(c => new carresponseDTO
            {
                ID = c.Id,
                Title = c.Title,
                ImageUrl = c.CarImage,
                Regnumber = c.RegistorNo,
                Brand = c.Brand,
          
                Description = c.Description,
                Model = c.Model,
                IsAvailable = c.IsAvailable,
            }).ToList();
        }

        public async Task<carresponseDTO> EditCar(Guid carid, carrequestDTO carRequest)
        {
        
            var car = await _carrepository.GetCarById(carid);
            if (car == null) return null;


            car.Title = carRequest.Title;
            car.RegistorNo = carRequest.Regnumber;
            car.Brand = carRequest.Brand;
            car.Description = carRequest.Description;
            car.Model = carRequest.Model;

            if (carRequest.ImageFile != null && carRequest.ImageFile.Length > 0)
            {
    
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, car.CarImage.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

               
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(carRequest.ImageFile.FileName);
                var newImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "carimages", fileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await carRequest.ImageFile.CopyToAsync(stream);
                }

                car.CarImage = "/carimages/" + fileName;
            }

            await _carrepository.EditCar(car);

  
            return new carresponseDTO
            {
                ID = car.Id,
                Title = car.Title,
                ImageUrl = car.CarImage,
                Regnumber = car.RegistorNo,
                Brand = car.Brand,
                Description = car.Description,
                Model = car.Model,
                IsAvailable = car.IsAvailable,
            };
        }

        public async Task<bool> DeleteCarAsync(Guid carId)
        {
          
            var car = await _carrepository.GetCarById(carId);
            if (car == null) return false;

            if (!string.IsNullOrEmpty(car.CarImage))
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, car.CarImage.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            await _carrepository.DeleteCar(carId);
            return true;
        }



    }
}
