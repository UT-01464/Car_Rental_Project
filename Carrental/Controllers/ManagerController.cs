using Carrental.Dtos.RequestDTO;
using Carrental.IsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        public readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("GetAllRentals")]
        public async Task<IActionResult> GetAllRentals()
        {
            try
            {
                var result = await _managerService.GetAllRentals();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("RentalByID/{id}")]
        public async Task<IActionResult> GetRentalById(Guid id)
        {
            try
            {
                var result = await _managerService.GetRentalById(id);
                if (result == null)
                    return NotFound("Rental not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("rentals/customer/{customerId}")]
        public async Task<IActionResult> GetAllRentalsByCustomerId(Guid customerId)
        {
            try
            {
                var result = await _managerService.GetAllRentalsByCustomerId(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("AddRental")]
        public async Task<IActionResult> AddRental(RentalRequestDTO rentalRequestDTO)
        {
            try
            {
                var result = await _managerService.AddRental(rentalRequestDTO);
                return CreatedAtAction(nameof(GetRentalById), new { id = result.id }, result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("AcceptRental/{id}")]
        public async Task<IActionResult> AcceptRental(Guid id)
        {
            try
            {
                var result = await _managerService.AcceptRental(id);
                if (result == null)
                    return NotFound("Rental not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("RejectRental/{rentalid}")]
        public async Task<IActionResult> RejectRental(Guid rentalid)
        {
            var isDeleted = await _managerService.RejectRental(rentalid);
            if (!isDeleted) return NotFound();

            return Ok("sccessfully deleted");
        }

        [HttpPut("bike-return/{id}")]
        public async Task<IActionResult> UpdateReturn(Guid id)
        {
            try
            {
                var result = await _managerService.UpdateReturn(id);
                if (result == null)
                    return NotFound("Rental not found or already returned");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("CheckAndUpdateOverdueRentals")]

        public async Task<IActionResult> CheckAndUpdateOverdueRentals()
        {
            var overdue = await _managerService.CheckAndUpdateOverdueRentals();
            if (overdue == null) return NotFound();
            return Ok(overdue);
        }
    }
}
