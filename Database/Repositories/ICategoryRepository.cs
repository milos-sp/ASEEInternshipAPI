using ProductAPI.Commands;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public interface ICategoryRepository
    {
        Task<int> InsertBulkCategories(List<CategoryEntity> categories);

        Task<CategoryEntity> GetCategoryByCodeAsync(string code);

        Task<CategoryEntity?> UpdateCategoryAsync(CreateCategoryCommand category);

        Task<CategoryEntity> InsertCategoryAsync(CategoryEntity category);

        Task<List<CategoryEntity>> GetAllCategoriesAsync(string parentCode);
    }
}
