using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public OrdersController(
            ApplicationDbContext dbContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: /Orders/
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            int id = await GetCurrentUserIdAsync();
            var orders = _dbContext.Orders.Where(o => o.UserId == id).ToList();
            return Ok(_mapper.Map<List<OrderDTO>>(orders));
        }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception("No user found");
            }

            return user.Id;
        }

        // GET: /Orders/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            int userId = await GetCurrentUserIdAsync();
            var result = await _dbContext.Orders.FirstOrDefaultAsync(pc => pc.Id == id);
            if (result == null)
            {
                return BadRequest($"No such order id = '{id}'");
            }

            return Ok(_mapper.Map<OrderDTO>(result));
        }

        // POST: /Orders/
        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var order = _mapper.Map<Order>(orderDto);
            await FillOrderAsync(order);
            order.State = OrderStates.New;
            order.UserId = await GetCurrentUserIdAsync();

            var r = await _dbContext.Orders.AddAsync(order);
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
