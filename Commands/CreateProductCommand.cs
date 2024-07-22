using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Commands
{
    public class CreateProductCommand
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ProductKind Kind { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public bool IsPackage { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailabilityEnd { get; set; }
        public double Price { get; set; }
    }
}
