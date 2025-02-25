using PranayChauhanProjectAPI.Models.Domain;

namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface ICategoryRepository
    {

        Task<Category> CreateAsync (Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid Id);
        

        Task<Category?> UpdateById( Category category);

        Task<Category?> DeleteCategoryById(Guid id);
    }
}
