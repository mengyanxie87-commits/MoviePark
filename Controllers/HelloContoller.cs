using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;
namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult HelloWorld()
        {
            return Ok("Hello, World!");
        }

        [HttpPost("hello")]
        public IActionResult HelloWorldPost()
        {
            return Ok("Hello from POST!");
        }

        [RouteAttribute("helloentity")]
        [HttpGetAttribute]
        [ProducesAttribute("application/json")]
        public Message helloMessage()
        {
            Message hello = new Message("Hello World!");
            hello.Title = "Hello World!我是實體物件";
            hello.code = 200;
            return hello;
        }
        [HttpGetAttribute]
        [RouteAttribute("sayhello/qrystring")]
        [Produces("text/plain")]
        public string helloworldQueryString([FromQueryAttribute(Name = "w")] string who)
        {
            string content = $"{who}您好";
            return content;
        }

        [HttpGet]
        [Route("sayHello/path2/{w}")]
        [Produces("text/plain")]
        public string helloWorldPathAsParameter2([FromRoute(Name = "w")] string who)
        {
            string content = $"{who} 您好";
            return content;
        }


    }
}