using ProductAPI.Commands;

namespace ProductAPI.Services
{
    public interface ICategoryService
    {
        Task<bool> InsertCategories(IEnumerable<CreateCategoryCommand> categories);

    }
}
