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

            var order = _mapper.Map<Order>(orderDto);
            await FillOrderAsync(order);
            order.UserId = User.GetId();
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

            var order = _mapper.Map<Order>(orderDto);
            await FillOrderAsync(order);
            return Ok(_mapper.Map<OrderDTO>(order));
        }

        private async Task FillOrderAsync(Order order)
        {
            decimal total = 0;
            foreach (var ci in order.CartItems)
            {
                ci.Product = await _dbContext.ProductItems.FirstOrDefaultAsync(p => p.Id == ci.ProductId);
                total += ci.Product.Price * ci.Quantity;
            }

            order.TotalPrice = total;
        }
    }
}
