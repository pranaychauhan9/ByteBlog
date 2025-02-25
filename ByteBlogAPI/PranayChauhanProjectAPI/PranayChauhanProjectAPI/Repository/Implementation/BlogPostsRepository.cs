using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using PranayChauhanProjectAPI.Data;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Repository.Implementation
{
    public class BlogPostsRepository : IBlogPostsRepository
    {

        private readonly ApplicationDbContext dbContext;
        public BlogPostsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogsPost> CreateAsync(BlogsPost blogs)
        {
            await dbContext.BlogsPosts.AddAsync(blogs);
            await dbContext.SaveChangesAsync();

            return blogs;
        }

        public async Task<IEnumerable<BlogsPost>> GetAllSync()
        {
            return await dbContext.BlogsPosts.Include(X => X.Categories).ToListAsync();
        }

        public async Task<BlogsPost?> GetByIdAsync(Guid id)
        {

            return await dbContext.BlogsPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogsPost?> UpdateAsync(BlogsPost blogs)
        {

            var existingBlogPost = dbContext.BlogsPosts.Include(x => x.Categories).FirstOrDefault(x => x.Id == blogs.Id);
            if (existingBlogPost is  null)
            {
                return null;
               
            }

            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogs);

            existingBlogPost.Categories = blogs.Categories;
            await dbContext.SaveChangesAsync();
            return blogs; ;
        }

        public async Task<BlogsPost?> DeleteByIdAsync(Guid id)
        {
                 var existingBlogPost = dbContext.BlogsPosts.FirstOrDefault(x => x.Id == id);


            if(existingBlogPost is null)
            {
                return null;
            }

             dbContext.BlogsPosts.Remove(existingBlogPost);
            await dbContext.SaveChangesAsync();

            return existingBlogPost;
        }

        public async Task<BlogsPost?> GetByUrlHandle(string urlHandle)
        {
            return await dbContext.BlogsPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
