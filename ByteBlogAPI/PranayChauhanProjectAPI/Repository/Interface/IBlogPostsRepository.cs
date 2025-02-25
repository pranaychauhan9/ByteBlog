using PranayChauhanProjectAPI.Models.Domain;

namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface IBlogPostsRepository



    {

        Task<IEnumerable<BlogsPost>> GetAllSync();
        Task<BlogsPost> CreateAsync(BlogsPost blogs);

        Task<BlogsPost?> GetByIdAsync(Guid id);
        Task<BlogsPost?> GetByUrlHandle(string urlHandle);

        Task<BlogsPost?> UpdateAsync( BlogsPost blogs);
        Task<BlogsPost?> DeleteByIdAsync(Guid id);
    }
}
