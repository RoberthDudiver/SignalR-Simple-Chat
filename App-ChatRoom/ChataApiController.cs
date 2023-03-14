using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_ChatRoom
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChataApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ChatData chatData)
        {
            // Aquí puede hacer lo que desee con los datos del chat, por ejemplo, guardarlos en una base de datos.
            return Ok(chatData);
        }
        [HttpGet]
        public IActionResult Get([FromQuery] string id)
        {
            // Aquí puede hacer lo que desee con los datos del chat, por ejemplo, guardarlos en una base de datos.
            return Ok(id);
        }

    }

    public class ChatData
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}


    