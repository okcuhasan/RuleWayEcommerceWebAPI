using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        public CategoryController(IGenericRepository<Category> genericRepository, ApplicationDbContext context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var allCategories = await _context.Categories
                .Include(x => x.Products)
                .Select(x => new
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    MinimumStockQuantity = x.MinimumStockQuantity,
                    RelevantProducts = x.Products.Select(x => new
                    {
                        ProductId = x.ProductId,
                        Title = x.Title,
                        Description = x.Description,
                        StockQuantity = x.StockQuantity,
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.CategoryName
                    })
                    .ToList()
                }).ToListAsync();

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
                    CategoryName = dtoFields.CategoryName,
                    MinimumStockQuantity = dtoFields.MinimumStockQuantity,
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
                    findedCategory.MinimumStockQuantity = dtoFields.MinimumStockQuantity;

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
