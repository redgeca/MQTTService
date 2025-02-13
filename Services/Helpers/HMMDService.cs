using IoTFallServer.Models;
using IoTFallServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IoTFallServer.Services.Helpers
{
    public class HMMDService : IHMMDService
    {
        private readonly IoTDBContext dbContext;
        private readonly ILogger<HMMDService> logger;

        public HMMDService(IoTDBContext context, ILogger<HMMDService> logger)
        {
            dbContext = context;
            this.logger = logger;
        }

        //public bool AddHMMDSensorValue(HMMDSensor sensorValue)
        //{
        //    if (sensorValue.header == 0xF1F2F3F4 && sensorValue.footer == 0xF5F6F7F8)
        //    {
        //        sensorValue.dateTime = DateTime.Now;
        //        dbContext.Add<HMMDSensor>(sensorValue);
        //        dbContext.SaveChanges();
        //        return true;
        //    } else
        //    {
        //        logger.LogError("Invalid sensor values : " + sensorValue);
        //        return false;
        //    }

        //}
    }
}
