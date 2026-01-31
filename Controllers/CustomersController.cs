using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyApiProject.Models;
using MyApiProject.Services;
using Microsoft.AspNetCore.Cors;
using System;

namespace MyApiProject.Controllers
{
    /// <summary>
    /// API controller for managing customer data.
    /// </summary>
    [EnableCors(PolicyName = "mydomain")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomersServiceImp _customersService;
        private readonly NorthwndContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customersService">The customer service implementation.</param>
        /// <param name="context">The database context.</param>
        public CustomersController(CustomersServiceImp customersService, NorthwndContext context)
        {
            _customersService = customersService ?? throw new System.ArgumentNullException(nameof(customersService));
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get all customer data.
        /// </summary>
        /// <returns>A JSON array of all customers.</returns>
        [HttpGet]
        [Route("all/rawdata")]
        [Produces("application/json")]
        public JsonResult GetAllCustomers()
        {
            var customers = _customersService.GetAllCustomers();
            return new JsonResult(customers);
        }

        /// <summary>
        /// Get customer data by ID.
        /// </summary>
        /// <param name="customerID">The ID of the customer.</param>
        /// <returns>A customer object if found, otherwise a 404 message.</returns>
        [HttpGet]
        [Route("qry/id/{cid}/rawdata")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Customers), 200)]
        [ProducesResponseType(typeof(Message), 404)]
        public IActionResult customersQryById([FromRoute(Name = "cid")] string customerID)
        {
            try
            {
                var result = _customersService.FindById(customerID);
                OkObjectResult ok = this.Ok(result);
                return ok;

            }
            catch (Exception)
            {
                return StatusCode(500, new Message { Status = "Error", Info = "Internal server error." });

            }
        }

        /// <summary>
        /// Add new customer data.
        /// </summary>
        /// <param name="customer">The customer object to add.</param>
        /// <returns>A success or error message.</returns>
        [HttpPost]
        [Route("add/rawdata")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(Message), 400)]
        public IActionResult AddCustomer([FromBody] Customers customer)
        {
            try
            {
                _customersService.AddCustomer(customer);
                return Ok(new Message { Status = "Success", Info = "Customer added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new Message { Status = "Error", Info = ex.Message });
            }
        }

        /// <summary>
        /// Update specific customer data.
        /// </summary>
        /// <param name="customerid">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer object.</param>
        /// <returns>A success or error message.</returns>
        [HttpPut]
        [Route("update/{customerid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(Message), 404)]
        [ProducesResponseType(typeof(Message), 400)]
        public IActionResult UpdateCustomer([FromRoute(Name = "customerid")] string customerid, [FromBody] Customers customer)
        {
            try
            {
                if (customerid != customer.CustomerID)
                {
                    return BadRequest(new Message { Status = "Error", Info = "Customer ID in route and body do not match." });
                }
                _customersService.UpdateCustomer(customer);
                return Ok(new Message { Status = "Success", Info = "Customer updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Message { Status = "Error", Info = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new Message { Status = "Error", Info = ex.Message });
            }
        }

        /// <summary>
        /// Delete specific customer data.
        /// </summary>
        /// <param name="customerid">The ID of the customer to delete.</param>
        /// <returns>A success or error message.</returns>
        [HttpDelete]
        [Route("delete/{customerid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(Message), 404)]
        [ProducesResponseType(typeof(Message), 400)]
        public IActionResult DeleteCustomer([FromRoute(Name = "customerid")] string customerid)
        {
            try
            {
                _customersService.DeleteCustomer(customerid);
                return Ok(new Message { Status = "Success", Info = "Customer deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Message { Status = "Error", Info = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new Message { Status = "Error", Info = ex.Message });
            }
        }
    }
}
