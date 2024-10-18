using Carrental.Dtos.RequestDTO;
using Carrental.IsServices;
using Carrental.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Carrental.Dtos.RequestDTO.LoginRequest;

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

        //[HttpPost]
        //public async Task<IActionResult> Add_customer(CustomerRequestDTO customerRequestDTO)
        //{
        //    var data=await _customerService.AddCustomer(customerRequestDTO);
        //    return Ok(data);
        //}

        [HttpPost("Add_customer")]
        public async Task<IActionResult> Add_customer(CustomerRequestDTO customerRequestDTO)
        {
            try
            {
                // Attempt to add the customer
                var data = await _customerService.AddCustomer(customerRequestDTO);
                return CreatedAtAction(nameof(GetCustomerById), new { id = data.Id }, data); // Use CreatedAtAction for new resources
            }
            catch (InvalidOperationException ex) // Catch specific exceptions for existing customers
            {
                return Conflict(new { message = ex.Message }); // Return a 409 Conflict response
            }
            catch (Exception ex) // Catch any other exceptions
            {
                // Log the exception if necessary
                return StatusCode(500, "An unexpected error occurred."); // Return a generic error response
            }
        }


        [HttpGet("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {       
                var result = await _customerService.GetAllCustomer();
                return Ok(result);     
           
        }

        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        //    {
        //        var result = await _customerService.UpdateCustomerByID(id, customerRequestDTO);
        //        if (result == null)
        //            return NotFound("Customer not found");
        //            return Ok(result);       

        //}

        [HttpPut("UpdateCustomerByID{id}")]
        public async Task<IActionResult> UpdateCustomerByID(Guid id, CustomerRequestDTO customerRequestDTO)
        {
            // Check if the incoming DTO is null
            if (customerRequestDTO == null)
            {
                return BadRequest("Customer data is required.");
            }

            // Call the validation method to check the properties of the DTO
            try
            {
                customerRequestDTO.Validate(); // Validate the DTO properties
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return the error message if validation fails
            }

            // Call the service to update the customer
            var result = await _customerService.UpdateCustomerByID(id, customerRequestDTO);

            // Check if the result is null (customer not found)
            if (result == null)
            {
                return NotFound("Customer not found");
            }

            return Ok(result); // Return the updated customer data
        }



        [HttpDelete("DeleteCustomer{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteCustomer(id);

            if (result)
                return NoContent(); 
            else
                return NotFound(); 
        }

        [HttpGet("GetbyId{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                return Ok(customer);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Password) || loginRequest.NIC == 0)
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                // Call the service layer to authenticate the customer
                var customer = await _customerService.AuthenticateCustomer(loginRequest.NIC, loginRequest.Password);

                if (customer != null)
                {
                    // Authentication successful
                    return Ok(customer);  // Return the authenticated customer data
                }
                else
                {
                    return Unauthorized("Invalid NIC or password.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during authentication
                return BadRequest(new { message = ex.Message });
            }
        }
    }


}
