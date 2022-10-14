using AspNetCore.Hashids.Mvc;
using dotnetconsulting.ApiHashIds.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetconsulting.ApiHashIds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private static readonly CustomerDto[] customers = new[]
        {
            new CustomerDto() { Id = 1, NonHashid=1, FirstName ="FirstName1", LastName="LastName1"} ,
            new CustomerDto() { Id = 2, NonHashid=2, FirstName = "FirstName2", LastName = "LastName2"},
            new CustomerDto() { Id = 3, NonHashid=3, FirstName = "FirstName3", LastName = "LastName3"}
        };

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetCustomers")]
        public IEnumerable<CustomerDto> Get()
        {
            return customers;
        }

        /// <summary>
        /// Im Standard kann Swagger das nicht umsetzen/ aufrufen
        /// https://localhost:7013/Customer/mEQm0aBg
        /// https://localhost:7013/Customer/7GQl7a1x
        /// https://localhost:7013/Customer/M9Qo4roD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:hashids}")]
        public ActionResult<CustomerDto> Get([ModelBinder(typeof(HashidsModelBinder))] int id)
        {
            return Ok(customers.SingleOrDefault(c => c.Id == id));
        }
    }
}