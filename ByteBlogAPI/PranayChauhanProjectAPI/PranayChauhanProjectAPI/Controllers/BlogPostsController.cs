using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PranayChauhanProjectAPI.DTO;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {

        private readonly IBlogPostsRepository blogPostsRepository;
        private readonly ICategoryRepository categoryRepository;
        public BlogPostsController(IBlogPostsRepository blogPostsRepository, ICategoryRepository categoryRepository)
        {

            this.blogPostsRepository = blogPostsRepository;
            this.categoryRepository = categoryRepository;

        }


        [HttpPost]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateBlog(CreateBlogPostsRequest request)
        {

            var blogs = new BlogsPost
            {

                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };


            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory is not null)
                {

                    blogs.Categories.Add(existingCategory);

                }

            }

            blogs = await blogPostsRepository.CreateAsync(blogs);

            var response = new BlogPostsDTO
            {

                Id = blogs.Id,
                Title = blogs.Title,
                UrlHandle = blogs.UrlHandle,
                Content = blogs.Content,
                ShortDescription = blogs.ShortDescription,
                PublishedDate = blogs.PublishedDate,
                Author = blogs.Author,
                FeaturedImageUrl = blogs.FeaturedImageUrl,
                IsVisible = blogs.IsVisible,
                Categories = blogs.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };

            return Ok(response);

        }

        [HttpGet]
       

        public async Task<IActionResult> GetAllBlogPost()

        {
            var blogs = await blogPostsRepository.GetAllSync();

            var response = new List<BlogPostsDTO>();

            foreach (var blogPost in blogs)
            {
                response.Add(new BlogPostsDTO
                {

                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });

            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetBlogPostById(Guid id)

        {

            var blogPost = await blogPostsRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostsDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };

            return Ok(response);

        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostsByUrlHandle(string urlHandle)
        {

            var blogPost = await blogPostsRepository.GetByUrlHandle(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostsDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()

            };

            return Ok(response);

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateBlogPostById(Guid id, UpdateBlogPostRequestDTO request)
        {
            var blogPost = new BlogsPost
            {

                Id = id,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()

            };


            foreach (var category in request.Categories)
            {
                var existinCategory = await categoryRepository.GetById(category);
                if (existinCategory is not null)
                {
                    blogPost.Categories.Add(existinCategory);
                }
            }

            blogPost = await blogPostsRepository.UpdateAsync(blogPost);

            if (blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostsDTO
            {

                Id = blogPost.Id,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };

            return Ok(response);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteblogPost(Guid id)

        {
            var deletedBlogPost = await blogPostsRepository.DeleteByIdAsync(id);

            if (deletedBlogPost == null)
            {
                return NotFound();
            }

            var resonse = new BlogPostsDTO
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                UrlHandle = deletedBlogPost.UrlHandle,
                Content = deletedBlogPost.Content,
                ShortDescription = deletedBlogPost.ShortDescription,
                PublishedDate = deletedBlogPost.PublishedDate,
                Author = deletedBlogPost.Author,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
            };

            return Ok(resonse);

        }
    }
}
