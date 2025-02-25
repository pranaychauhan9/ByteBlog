using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PranayChauhanProjectAPI.DTO;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase


    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]

        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {

            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {

                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,

                };
                blogImage = await imageRepository.Upload(file, blogImage);


                var response = new BlogImageDTO

                {
                    Id = blogImage.Id,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated
                };

                return Ok(response);

            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {

            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB ");
            }

        }


        [HttpGet]

        public async Task<IActionResult> GetAllImages()

        {

            var blogImages = await imageRepository.GetAllImages();

            var response = new List<BlogImageDTO>();

            foreach (var images in blogImages)
            {

                response.Add(new BlogImageDTO
                {   
                    Id = images.Id,
                    FileExtension = images.FileExtension,
                    FileName = images.FileName,
                    Title = images.Title,
                    DateCreated = images.DateCreated,
                    Url = images.Url


                });
            }
            return Ok(response);
        }
    }


}
