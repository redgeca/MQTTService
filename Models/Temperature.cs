using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzureIoTServer.Models
{
    public class Temperature
    {
        [Key]
        public DateTime dateTime { get; set; }

        [JsonInclude]
        [JsonPropertyName("temp")]
        public float temperature{ get; set; }
    }
}
