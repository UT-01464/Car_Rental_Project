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
            var data=await _customerService.AddCustomer(customerRequestDTO);
            return Ok(data);
        }
    }
}
