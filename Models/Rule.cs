using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    public class Rule
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("catcode")]
        public string Catcode { get; set; }
    }
}
