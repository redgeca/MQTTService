using IoTFallServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTFallServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HMMDController : Controller
    {
        private readonly IHMMDService hmmdService;

        public HMMDController(IHMMDService hmmdService)
        {
            this.hmmdService = hmmdService;
        }

        [Authorize]
        [HttpGet(Name = "getSensorValues")]
        public IActionResult Index()
        {
            return Ok("This is a string response.");
        }

        //[HttpPut(Name = "addSensorValue")]
        //public IActionResult Create(HMMDSensor sensorValue)
        //{
        //    if (hmmdService.AddHMMDSensorValue(sensorValue))
        //    {
        //        return Created("", Json(sensorValue));
        //    } else
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
