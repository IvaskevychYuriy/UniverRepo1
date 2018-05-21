using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebStore.Api.Constants;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = _dbContext.ProductCategories
                .AsNoTracking()
                .ProjectTo<ProductCategoryDTO>(_mapper.ConfigurationProvider);

            return Ok(result);
        }

        // GET: /ProductCategories/sub
        [AllowAnonymous]
        [HttpGet("sub")]
        public async Task<IActionResult> GetSub()
        {
            var result = _dbContext.ProductSubCategories
                .AsNoTracking()
                .ProjectTo<ProductSubCategoryDTO>(_mapper.ConfigurationProvider);

            return Ok(result);
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

        // GET: /ProductCategories/sub/{id}
        [AllowAnonymous]
        [HttpGet("sub/{id}")]
        public async Task<IActionResult> GetSub(int id)
        {
            var result = await _dbContext.ProductSubCategories.FirstOrDefaultAsync(pc => pc.Id == id);
            if (result == null)
            {
                return BadRequest($"No such product sub category with id = '{id}'");
            }

            return Ok(_mapper.Map<ProductSubCategoryDTO>(result));
        }

        // POST: /ProductCategories/
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDTO productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var category = _mapper.Map<ProductCategory>(productCategory);
            await _dbContext.ProductCategories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // POST: /ProductCategories/sub
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("sub")]
        public async Task<IActionResult> CreateSub([FromBody] ProductSubCategoryDTO productSubCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var subCategory = _mapper.Map<ProductSubCategory>(productSubCategory);
            await _dbContext.ProductSubCategories.AddAsync(subCategory);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
