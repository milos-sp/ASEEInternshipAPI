using Microsoft.EntityFrameworkCore;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        DatabaseContext _dbContext;
        public CategoryRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryEntity>> GetAllCategoriesAsync(string parentCode)
        {
            var query = _dbContext.Categories.AsQueryable();

            if (!String.IsNullOrEmpty(parentCode))
            {
                query = query.Where(s => s.ParentCode.Equals(parentCode));
            }

            var categories = await query.ToListAsync();

            return categories;
        }

        public async Task<CategoryEntity?> GetCategoryByCodeAsync(string code)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Code.Equals(code));
        }

        public async Task<int> InsertBulkCategories(List<CategoryEntity> categories)
        {
            int countTotal = 0;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false; // za bolje performanse
            foreach (CategoryEntity category in categories)
            {
                _dbContext.Categories.Add(category);
                countTotal++;
            }
            await _dbContext.SaveChangesAsync();

            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            return countTotal;
        }

        public async Task<CategoryEntity> InsertCategoryAsync(CategoryEntity category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<bool> IsTopLevelCategory(string code)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Code.Equals(code));
            return String.IsNullOrEmpty(category.ParentCode);
        }

        public async Task<CategoryEntity?> UpdateCategoryAsync(CreateCategoryCommand category)
        {
            var existingCategory = _dbContext.Categories.FirstOrDefault(x => x.Code.Equals(category.Code));

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.ParentCode = category.ParentCode;

            await _dbContext.SaveChangesAsync();

            return existingCategory;
        }
    }
}
