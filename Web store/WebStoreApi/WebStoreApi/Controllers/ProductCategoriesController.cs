using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductCategoriesController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        // GET: /ProductCategories/
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await _dbContext.ProductCategories.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductCategoryDTO>>(result));
        }

        // GET: /ProductCategories/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id);
            if (result == null)
            {
                return BadRequest($"No such product category with id = '{id}'");
            }

            return Ok(_mapper.Map<ProductCategoryDTO>(result));
        }

        // POST: /ProductCategories/
        [Authorize(Roles = "Administrator")]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDTO productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var category = _mapper.Map<ProductCategory>(productCategory);
            var r = await _dbContext.ProductCategories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ProductCategoryDTO>(category));
        }
    }
}
