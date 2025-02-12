using AzureIoTServer.Models;
using AzureIoTServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureIoTServer.Controllers
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
