﻿namespace ProductAPI.Models
{
    public class Product
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
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
