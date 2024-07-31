using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    public class Rule
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("catcode")]
        public string Catcode { get; set; }

        [JsonPropertyName("predicate")]
        public string Predicate { get; set; }
    }
}
