using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;
using WebStore.Api.Extensions;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;
using WebStore.Models.Models;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrdersController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /Orders/
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = _dbContext.Orders
                .AsNoTracking()
                .Where(o => o.UserId == User.GetId())
                .OrderByDescending(o => o.Id)
                .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider);

            return Ok(orders);
        }

        // GET: /Orders/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Orders.FirstOrDefaultAsync(pc => pc.Id == User.GetId());
            if (result == null)
            {
                return BadRequest($"No such order id = '{id}'");
            }

            return Ok(_mapper.Map<OrderDTO>(result));
        }

        // POST: /Orders/
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var order = new Order();
            orderDto.CartItems = orderDto.CartItems.OrderBy(ci => ci.Id).ToList();

            var ids = orderDto.CartItems.Select(ci => ci.Product.Id).ToList();
            var items = await _dbContext.ProductItems
                .AsNoTracking()
                .Where(pi => pi.Active && ids.Contains(pi.Id))
                .Select(pi => new
                {
                    Price = pi.Price,
                    AvailableCount = pi.StorageItems.Count() - pi.CartItems.Count()
                })
                .ToListAsync();
            
            decimal totalPrice = 0;
            for (int i = 0; i < ids.Count; ++i)
            {
                if (items[i].AvailableCount < orderDto.CartItems[i].Quantity)
                {
                    return BadRequest();
                }

                totalPrice += orderDto.CartItems[i].Quantity * items[i].Price;
                for (int c = 0; c < orderDto.CartItems[i].Quantity; ++c)
                {
                    order.CartItems.Add(new CartItem()
                    {
                        ProductId = ids[i],
                        ProductPrice = items[i].Price
                    });
                }
            }

            order.TotalPrice = totalPrice;
            order.UserId = User.GetId();
            order.Coordinates = _mapper.Map<AddressCoordinates>(orderDto.Coordinates);
            order.HistoryRecords.Add(new OrderHistory()
            {
                State = OrderStates.New
            });

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<OrderDTO>(order));
        }

        // POST: /Orders/calculate
        [AllowAnonymous]
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var order = new OrderDTO();
            orderDto.CartItems = orderDto.CartItems.OrderBy(ci => ci.Id).ToList();

            var ids = orderDto.CartItems.Select(ci => ci.Product.Id).ToList();
            var items = await _dbContext.ProductItems
                .AsNoTracking()
                .Where(pi => pi.Active && ids.Contains(pi.Id))
                .OrderBy(pi => pi.Id)
                .Select(pi => new
                {
                    Price = pi.Price,
                    Product = pi,
                    AvailableCount = pi.StorageItems.Count() - pi.CartItems.Count()
                })
                .ToListAsync();

            decimal totalPrice = 0;
            for (int i = 0; i < ids.Count; ++i)
            {
                if (items[i].AvailableCount < orderDto.CartItems[i].Quantity)
                {
                    return BadRequest();
                }

                totalPrice += orderDto.CartItems[i].Quantity * items[i].Price;
                order.CartItems.Add(new CartItemDTO()
                {
                    Product = _mapper.Map<ProductItemDTO>(items[i].Product),
                    Quantity = orderDto.CartItems[i].Quantity
                });
            }

            order.TotalPrice = totalPrice;
            return Ok(order);
        }
    }
}
