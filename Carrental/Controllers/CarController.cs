using Carrental.Dtos.RequestDTO;
using Carrental.IsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly Icarservice _icarservice;
        public CarController(Icarservice icarservice)
        {
            _icarservice = icarservice;
        }


        [HttpPost("Addcar")]
        public async Task<IActionResult> Addcar([FromForm] carrequestDTO request)
        {
            var result = await _icarservice.AddcarAsync(request);
            return Ok(result);
        }


        [HttpPut("{carId}")]
        public async Task<IActionResult> Editcar(Guid carId, [FromForm] carrequestDTO Request)
        {
            var updatedcar = await _icarservice.EditCar(carId, Request);
            if (updatedcar == null) return NotFound();

            return Ok(updatedcar);
        }


     

        [HttpGet("GetAllCar")]
        public async Task<IActionResult> GetAllCar()
        {
            var result = await _icarservice.GetAllCars();
            return Ok(result);
        }


        [HttpDelete("DeleteCar/{CarId}")]
        public async Task<IActionResult> DeleteCar(Guid carId)
        {
            var isDeleted = await _icarservice.DeleteCarAsync(carId);
            if (!isDeleted) return NotFound();

            return NoContent();
        }
    }
}
