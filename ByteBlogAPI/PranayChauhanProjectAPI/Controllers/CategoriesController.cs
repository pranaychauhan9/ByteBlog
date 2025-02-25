using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PranayChauhanProjectAPI.Data;
using PranayChauhanProjectAPI.DTO;
using PranayChauhanProjectAPI.Models.Domain;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Controllers


{

    //htttps:// localhost:xxxx//api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase

    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDTO request)
        {

            //Map To Domain Model

            var category = new Category
            {

                //Id Automaticaly created
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };


            await categoryRepository.CreateAsync(category);

            //Domain To DTO

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {

            // Domian To DTO
            var categories = await categoryRepository.GetAllAsync();

            var response = new List<CategoryDTO>();

            foreach (var category in categories)
            {

                response.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle

                });

            }

            return Ok(response);

        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {

            var exposeCategory = await categoryRepository.GetById(id);

            if (exposeCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDTO
            {
                Id = exposeCategory.Id,
                Name = exposeCategory.Name,
                UrlHandle = exposeCategory.UrlHandle,
            };


            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateCategoryById(Guid id, UpdateCategoryDTO updateCategory)
        {
            var category = new Category
            {
                //Id Automaticaly created
                Id = id,
                Name = updateCategory.Name,
                UrlHandle = updateCategory.UrlHandle,
            };

            category = await categoryRepository.UpdateById(category);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {

            var category = await categoryRepository.DeleteCategoryById(id);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle

            };

            return Ok(response);

        }
    }
}
