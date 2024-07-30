using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public interface ICategoryService
    {
        Task<bool> InsertCategories(IEnumerable<CreateCategoryCommand> categories);

        Task<List<Category>> GetAllCategories(string parentCode);

        Task<Category> GetCategoryByCode(string code);

        Task<bool> IsTopLevelCategory(string code);

    }
}
