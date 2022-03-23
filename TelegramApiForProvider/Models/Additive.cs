using Newtonsoft.Json;

namespace TelegramApiForProvider.Models
{
    public class Additive
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Cost")]
        public decimal Cost { get; set; }
    }
}
