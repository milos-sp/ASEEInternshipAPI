using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
using System.Data;

namespace ProductAPI.Database.Repositories
{
    public class ProductRepository : IProductRepository
    {
        ProductDbContext _dbContext;
        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity newProductEntity)
        {
            _dbContext.Products.Add(newProductEntity);
            await _dbContext.SaveChangesAsync();
            return newProductEntity;
        }

        public async Task DeleteProduct(ProductEntity productEnitity)
        {
            _dbContext.Products.Remove(productEnitity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductEntity> GetProductByCode(string productCode)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Code.Equals(productCode));
        }

        public async Task<PagedSortedList<ProductEntity>> GetProducts(int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null)
        {

            var query = _dbContext.Products.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "code":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Code) : query.OrderByDescending(x => x.Code);
                        break;
                    case "name":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var products = await query.ToListAsync();

            return new PagedSortedList<ProductEntity>
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Items = products
            };
        }
    }
}
