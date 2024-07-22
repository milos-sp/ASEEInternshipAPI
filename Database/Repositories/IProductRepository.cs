using Microsoft.AspNetCore.Mvc;
using ProductAPI.Database.Entities;
using ProductAPI.Models;

namespace ProductAPI.Database.Repositories
{
    public interface IProductRepository
    {
        Task<ProductEntity> CreateProduct(ProductEntity newProductEntity);
        Task DeleteProduct(ProductEntity productEnitity);
        Task<ProductEntity> GetProductByCode(string productCode);
        Task<PagedSortedList<ProductEntity>> GetProducts(int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null);
    }
}
