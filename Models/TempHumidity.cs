using System.ComponentModel.DataAnnotations;

namespace AzureIoTServer.Models
{
    public class TempHumidity
    {
        [Key]
        public DateTime dateTime { get; set; }

        public float temperature{ get; set; }

        public float humidity { get; set; }
    }
}
