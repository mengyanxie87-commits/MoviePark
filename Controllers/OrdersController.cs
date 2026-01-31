using Microsoft.AspNetCore.Mvc;
namespace MyApiProject.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly ConnectionFactory _databaseAccess;

        public OrdersController()
        {
            _databaseAccess = _databaseAccess;
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("qry/{id:int}/rawdata")]
        public Int32 OrdersQry([FromRoute(Name = "id")] Int32 orderId)
        {
            return orderId;

        }
        [HttpGet("qry/date/{date}")]
        [Produces("application/json")]

        public IActionResult OrdersQryByDate([FromRoute] string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var orderDate))
            {
                return Ok(orderDate); // 回傳 200 + JSON
            }

            return BadRequest("日期格式必須是 yyyy-MM-dd"); // 回傳 400
        }

      
    }
}