using Microsoft.EntityFrameworkCore;
using PranayChauhanProjectAPI.Data;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Repository.Implementation
{
    public class ImageRepository : IImageRepository


    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
                
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllImages()

        {
           return  await dbContext.BlogImage.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // 1 Upload Image to API/Images

            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);

            await file.CopyToAsync(stream);


            //2 Update Database
            //https://pc.com/images/pcworld.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await dbContext.BlogImage.AddAsync(blogImage);

            await dbContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
