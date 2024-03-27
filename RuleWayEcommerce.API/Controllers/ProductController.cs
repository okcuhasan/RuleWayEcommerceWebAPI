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
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly ApplicationDbContext _context;
        public ProductController(IGenericRepository<Product> genericRepository, ApplicationDbContext context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var allProducts = await _genericRepository.GetAll();

            return Ok(allProducts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedProduct = await _genericRepository.GetById(id);
                if (findedProduct == null)
                {
                    return NotFound("The relevant product was not found");
                }
                else
                {
                    return Ok(findedProduct);
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid data entered");
            }
            else
            {
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                if (category == null)
                {
                    return NotFound("No such category found");
                }
                else
                {
                    var product = new Product
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        StockQuantity = dto.StockQuantity,
                        CategoryId = dto.CategoryId,
                    };


                    await _genericRepository.Add(product);
                    return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
                }
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO dtoFields)
        {
            if (id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedProduct = await _genericRepository.GetById(id);

                if (findedProduct == null)
                {
                    return NotFound("The relevant product was not found");
                }
                else
                {
                    var category = await _context.Categories.FindAsync(dtoFields.CategoryId);
                    if (category == null)
                    {
                        return NotFound("No such category found");
                    }
                    else
                    {
                        findedProduct.Title = dtoFields.Title;
                        findedProduct.Description = dtoFields.Description;
                        findedProduct.StockQuantity = dtoFields.StockQuantity;
                        findedProduct.CategoryId = dtoFields.CategoryId;

                        await _genericRepository.Update(findedProduct);
                        return NoContent();
                    }
                }
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return NotFound("Id cannot be zero");
            }
            else
            {
                var findedProduct = await _genericRepository.GetById(id);
                if (findedProduct == null)
                {
                    return NotFound("The relevant product was not found");
                }
                else
                {
                    await _genericRepository.Delete(findedProduct);
                }
            }
            return NoContent();
        }



        [HttpPost("filterProductsByAll")]
        public async Task<IActionResult> FilterProduct(string searchProductsByFilters)
        {
            if (searchProductsByFilters == null)
            {
                return BadRequest("To search for a product, enter title, description, or category");
            }
            else
            {
                var products = await _context.Products
                    .Where(x => x.Title == searchProductsByFilters ||
                                x.Description == searchProductsByFilters ||
                                x.Category.CategoryName == searchProductsByFilters)
                    .ToListAsync();

                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No such product found");
                }
            }
        }

        [HttpPost("filterProductsByTitle")]
        public async Task<IActionResult> FilterProductByTitle(string searchByTitle)
        {
            if (searchByTitle == null)
            {
                return BadRequest("To search for a product, enter title");
            }
            else
            {
                var products = await _context.Products
                    .Where(x => x.Title == searchByTitle)
                    .ToListAsync();

                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No such product found");
                }
            }
        }

        [HttpPost("filterProductsByDescription")]
        public async Task<IActionResult> FilterProductByDescription(string searchByDescription)
        {
            if (searchByDescription == null)
            {
                return BadRequest("To search for a product, enter description");
            }
            else
            {
                var products = await _context.Products
                    .Where(x => x.Description == searchByDescription)
                    .ToListAsync();

                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No such product found");
                }
            }
        }


        [HttpPost("filterProductsByCategoryName")]
        public async Task<IActionResult> FilterProductByCategoryName(string searchByCategory)
        {
            if (searchByCategory == null)
            {
                return BadRequest("To search for a product, enter category name");
            }
            else
            {
                var products = await _context.Products
                    .Where(x => x.Category.CategoryName == searchByCategory)
                    .ToListAsync();

                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No such product found");
                }
            }
        }
    }
}
