using Carrental.Dtos.RequestDTO;
using Carrental.IsServices;
using Carrental.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Add_customer(CustomerRequestDTO customerRequestDTO)
        {
            var data = await _customerService.AddCustomer(customerRequestDTO);
            return Ok(data);
        }

        [HttpGet("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var result = await _customerService.GetAllCustomer();
            return Ok(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        {
            var result = await _customerService.UpdateCustomerByID(id, customerRequestDTO);
            if (result == null)
                return NotFound("Customer not found");
            return Ok(result);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteCustomer(id);

            if (result)
                return NoContent();
            else
                return NotFound();
        }



    }


}