using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuleWayEcommerce.Data.DTO;
using RuleWayEcommerce.Data.Entity;
using RuleWayEcommerce.Service.Abstracts;

namespace RuleWayEcommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _genericRepository;
        public CategoryController(IGenericRepository<Category> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var allCategories = await _genericRepository.GetAll();

            return Ok(allCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if(id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedCategory = await _genericRepository.GetById(id);
                if(findedCategory == null)
                {
                    return NotFound("The relevant category was not found");
                }
                else
                {
                    return Ok(findedCategory);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO dtoFields)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("invalid data entered");
            }
            else
            {
                var category = new Category
                {
                    CategoryId = dtoFields.CategoryId,
                    CategoryName = dtoFields.CategoryName,
                };
                await _genericRepository.Add(category);

                return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId}, category);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO dtoFields)
        {
            if (id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedCategory = await _genericRepository.GetById(id);

                if(findedCategory == null)
                {
                    return NotFound("The relevant category was not found");
                }
                else
                {
                    findedCategory.CategoryName = dtoFields.CategoryName;

                    await _genericRepository.Update(findedCategory);
                    return NoContent();
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedCategory = await _genericRepository.GetById(id);
                if(findedCategory == null)
                {
                    return NotFound("The relevant category was not found");
                }
                else
                {
                    await _genericRepository.Delete(findedCategory);
                }
            }
            return NoContent();
        }


    }
}
