using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Api.Constants;
using WebStore.Api.DataTransferObjects;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductItemsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductItemsController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /ProductItems/Get?categoryId=
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int? categoryId = null, int? subCategoryId = null, int? pageSize = null, int page = 1)
        {
            var result = _dbContext.ProductItems.AsQueryable();
            if (subCategoryId != null)
            {
                result = result.Where(pi => pi.SubCategoryId == subCategoryId);
            }
            else if (categoryId != null)
            {
                result = result.Where(pi => pi.SubCategory.CategoryId == categoryId);
            }
            int count = result.Count();

            if (pageSize != null)
            {
                if (page > 1)
                {
                    result = result.Skip((page - 1) * (int)pageSize);
                }
                result = result.Take((int)pageSize);
            }

            return Ok(new PageDataDTO()
            {
                ProductItems = await result.ProjectTo<ProductItemDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                TotalCount = count
            });
        }

        // GET: /ProductItems/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.ProductItems.FirstOrDefaultAsync(pi => pi.Id == id);
            if (result == null)
            {
                return BadRequest($"No such product category with id = '{id}'");
            }

            return Ok(_mapper.Map<ProductItemDTO>(result));
        }

        // POST: /ProductItems/
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductItemDTO productItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var product = _mapper.Map<ProductItem>(productItem);
            await _dbContext.ProductItems.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ProductItemDTO>(product));
        }

        // PUT: /ProductItems
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductItemDTO productItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var product = _dbContext.ProductItems.FindAsync(productItem.Id);
            if (product == null)
            {
                return BadRequest($"No such product with id = '{productItem.Id}'");
            }

            await _mapper.Map(productItem, product);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

