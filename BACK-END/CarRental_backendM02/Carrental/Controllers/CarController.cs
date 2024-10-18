//using Carrental.Dtos.RequestDTO;
//using Carrental.IsServices;
//using Carrental.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;




//namespace Carrental.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CarController : ControllerBase
//    {
//        private readonly Icarservice _icarservice;
//        public CarController(Icarservice icarservice)
//        {
//            _icarservice = icarservice;
//        }


//        [HttpPost("add")]
//        public async Task<IActionResult> AddCar([FromForm] carrequestDTO request)
//        {
//            // Basic validation for required fields (Title, Regnumber, etc.)
//            if (string.IsNullOrEmpty(request.Title))
//            {
//                return BadRequest("Title is required.");
//            }

//            if (string.IsNullOrEmpty(request.Regnumber))
//            {
//                return BadRequest("Registration number is required.");
//            }

//            if (request.ImageFile != null && request.ImageFile.Length > 5 * 1024 * 1024) // Limit the image size to 5MB
//            {
//                return BadRequest("Image file size should not exceed 5MB.");
//            }

//            try
//            {
//                // Call the service to add the car and return the response
//                var carResponse = await _icarservice.AddCarAsync(request);

//                return Ok(carResponse); // Respond with 200 OK and the response DTO
//            }
//            catch (ArgumentNullException ex)
//            {
//                return BadRequest(ex.Message); // Respond with 400 Bad Request for invalid input
//            }
//            catch (Exception ex)
//            {
//                // Log the error for further investigation (optional logging)
//                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the car.");
//            }
//        }



//        [HttpPut("{carId}")]
//        public async Task<IActionResult> Editcar(Guid carId, [FromForm] carrequestDTO Request)
//        {
//            var updatedcar = await _icarservice.EditCar(carId, Request);
//            if (updatedcar == null) return NotFound();

//            return Ok(updatedcar);
//        }




//        [HttpGet("GetAllCar")]
//        public async Task<IActionResult> GetAllCar()
//        {
//            var result = await _icarservice.GetAllCars();
//            return Ok(result);
//        }


//        [HttpDelete("DeleteCar/{CarId}")]
//        public async Task<IActionResult> DeleteCar(Guid carId)
//        {
//            var isDeleted = await _icarservice.DeleteCarAsync(carId);
//            if (!isDeleted) return NotFound();

//            return NoContent();
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Carrental.IServices;
using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carrental.Dtos.ResponceDTO;

namespace Carrental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost("Addcar")]
        public async Task<IActionResult> AddCar([FromForm] CarRequestDTO carRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carResponse = await _carService.AddCarAsync(carRequestDTO, carRequestDTO.ImageFile);
            return CreatedAtAction(nameof(GetCarById), new { id = carResponse.ID }, carResponse);
        }

        [HttpGet("GetCarById/{id}")]
        public async Task<ActionResult<CarResponseDTO>> GetCarById(Guid id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPut("EditCar/{id}")]
        public async Task<IActionResult> EditCar(Guid id, [FromForm] CarRequestDTO carRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCar = await _carService.EditCar(id, carRequestDTO, carRequestDTO.ImageFile);
            return Ok(updatedCar);
        }


        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            string resultMessage = await _carService.DeleteCarAsync(id);

            // Return a message based on the result
            if (resultMessage.Contains("deleted successfully"))
            {
                return Ok(resultMessage); // Return success message with 200 OK status
            }
            else if (resultMessage.Contains("not found"))
            {
                return NotFound(resultMessage); // Return 404 with a not found message
            }

            return BadRequest(resultMessage); // Return a bad request if something went wrong
        }


        [HttpGet("GetAllCars")]
        public async Task<ActionResult<List<CarResponseDTO>>> GetAllCars()
        {
            var cars = await _carService.GetAllCars();
            return Ok(cars);
        }
    }
}

