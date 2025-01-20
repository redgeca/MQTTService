using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AzureIoTServer.Models
{
    public class ESP32CamLogs
    {
        [Key]
        public DateTime dateTime { get; set; }
        [Required]
        public string logMessage { get; set; }

        public ESP32CamLogs(string logMessage)
        {
            this.dateTime = DateTime.Now;
            this.logMessage = logMessage;
        }


    }
}
