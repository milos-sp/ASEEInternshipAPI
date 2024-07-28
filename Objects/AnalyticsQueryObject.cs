using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Objects
{
    public class AnalyticsQueryObject
    {
        [BindProperty(Name = "catcode")]
        public string? Catcode { get; set; }

        [BindProperty(Name = "start-date")]
        public DateTime? StartDate { get; set; }

        [BindProperty(Name = "end-date")]
        public DateTime? EndDate { get; set; }

        [BindProperty(Name = "direction")]
        public Directions? Direction { get; set; }
    }
}
