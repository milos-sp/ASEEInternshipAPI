using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Database.Repositories;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        private readonly IMapper _mapper;

        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task<bool> CheckIfProductExistsAsync(string code)
        {
            var category = await _repository.GetCategoryByCodeAsync(code);
            if (category == null)
            {
                return false;

            }
            return true;
        }
        public async Task<bool> InsertCategories(IEnumerable<CreateCategoryCommand> categories)
        {
            int index = 1;
            // List<CategoryEntity> categoryList = new List<CategoryEntity>();
            foreach (var category in categories)
            {
                var categoryExists = await CheckIfProductExistsAsync(category.Code);

                if (categoryExists)
                {
                    // potrebno azurirati name i parent code
                    await _repository.UpdateCategoryAsync(category);
                } else
                {
                    if (category.Code.Equals(""))
                    {
                        _logger.LogWarning("Row " + index + ": Code field is missing");
                    } else if (category.Name.Equals(""))
                    {
                        _logger.LogWarning("Row " + index + ": Name field is missing");
                    } else
                    {
                        var newCategoryEntity = _mapper.Map<CategoryEntity>(category);
                        // categoryList.Add(newCategoryEntity);
                        // mozda ipak treba odmah dodavati zbog update dela ako su isti kodovi u jednom fajlu
                        await _repository.InsertCategoryAsync(newCategoryEntity);
                    }
                }

                index++;
            }

            // int count = await _repository.InsertBulkCategories(categoryList);

            // _logger.LogInformation($"Number of inserted rows: {count}");

            return true;
        }

        public async Task<List<Category>> GetAllCategories(string parentCode)
        {
            var categories = await _repository.GetAllCategoriesAsync(parentCode);

            return _mapper.Map<List<Category>>(categories);
        }

        public async Task<Category> GetCategoryByCode(string code)
        {
            var category = await _repository.GetCategoryByCodeAsync(code);
            return _mapper.Map<Category>(category);
        }
    }
}
