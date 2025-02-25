using PranayChauhanProjectAPI.Models.Domain;

namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface IImageRepository
    {

        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAllImages();
    }
}
