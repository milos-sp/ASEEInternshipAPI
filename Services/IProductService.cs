using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(CreateProductCommand createProductCommand);
        Task<bool> DeleteProduct(string productCode);
        Task<Product> GetProduct(string productCode);
        Task<PagedSortedList<Product>> GetProducts(int page, int pageSize, SortOrder sortOrder, string? sortBy);
    }
}
