using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Api.Constants;
using WebStore.Api.DataTransferObjects;
using WebStore.Api.Models;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoragesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public StoragesController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: api/<controller>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = _dbContext.Storages
                .AsNoTracking()
                .ProjectTo<StorageListItemDTO>(_mapper.ConfigurationProvider);

            return Ok(result);
        }

        // GET api/<controller>/5
        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Storages
                .AsNoTracking()
                .Select(s => new StorageGroupedModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Coordinates = s.Coordinates,
                    Drones = s.Drones.ToList(),
                })
                .FirstOrDefaultAsync(s => s.Id == id);
            
            result.Items = _dbContext.StorageItems
                .AsNoTracking()
                .Where(si => si.StorageId == id && si.State == StorageItemStates.Available)
                .ToList()
                .GroupBy(si => new { si.ProductId, si.Price })
                .Join(_dbContext.ProductItems
                    .AsNoTracking()
                    .Select(pi => new
                    {
                        Product = pi,
                        AvailableCount = pi.StorageItems.Count() - pi.CartItems.Count()
                    }), 
                    g => g.Key.ProductId, x => x.Product.Id, (g, x) => new { g, x })
                .Select(joined => new StorageGroupedItemModel()
                {
                    ProductId = joined.g.Key.ProductId,
                    Price = joined.g.Key.Price,
                    Product = joined.x.Product,
                    AvailableCount = joined.x.AvailableCount,
                    Quantity = joined.g.Count()
                })
                .ToList();
            
            return Ok(_mapper.Map<StorageDTO>(result));
        }

        // POST api/<controller>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StorageListItemDTO value)
        {
            var storage = _mapper.Map<Storage>(value);
            await _dbContext.Storages.AddAsync(storage);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // POST api/<controller>/item
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("item")]
        public async Task<IActionResult> PostItem([FromBody]StorageItemDTO value)
        {
            for (int i = 0; i < value.Quantity; ++i)
            {
                var item = _mapper.Map<StorageItem>(value);
                item.CartItemId = null;
                item.State = StorageItemStates.Available;
                await _dbContext.StorageItems.AddAsync(item);
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var storage = await _dbContext.Storages.FirstOrDefaultAsync(s => s.Id == id);
            if (storage == null)
            {
                return BadRequest();
            }

            _dbContext.Storages.Remove(storage);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<controller>/5/items?productId=2
        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("{id}/items")]
        public async Task<IActionResult> DeleteItems(int id, int productId)
        {
            var items = await _dbContext.StorageItems
                .Where(si => si.StorageId == id && si.ProductId == productId && si.State == StorageItemStates.Available && si.CartItemId == null)
                .ToListAsync();

            var availableCount = await _dbContext.ProductItems
                .AsNoTracking()
                .Where(pi => pi.Id == productId)
                .Select(pi => pi.StorageItems.Count() - pi.CartItems.Count())
                .FirstOrDefaultAsync();

            if (items == null || items.Count == 0)
            {
                return Ok();
            }

            if (items.Count > availableCount)
            {
                return BadRequest();
            }
            
            _dbContext.StorageItems.RemoveRange(items);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
