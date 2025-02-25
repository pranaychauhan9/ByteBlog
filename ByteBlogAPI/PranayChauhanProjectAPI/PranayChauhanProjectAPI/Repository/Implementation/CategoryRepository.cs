using Microsoft.EntityFrameworkCore;
using PranayChauhanProjectAPI.Data;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteCategoryById(Guid id)
        {
            var existCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existCategory is null)
            {
                return null;
            }

            dbContext.Categories.Remove(existCategory);
            await dbContext.SaveChangesAsync();
            return existCategory;

        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {

            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateById(Category category)
        {

            var exposeCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (exposeCategory != null)
            {

                dbContext.Entry(exposeCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();

                return category;

            }
            return null;


        }
    }
}
