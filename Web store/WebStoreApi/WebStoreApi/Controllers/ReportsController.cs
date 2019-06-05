using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;
using WebStore.Api.Models;
using WebStore.DAL.Contexts;
using WebStore.Models.Enumerations;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReportsController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: api/<controller>/products
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProducts(DatesRangeModel range)
        {
            range = range ?? new DatesRangeModel();

            var result = _dbContext.Orders
                .AsNoTracking()
                .Where(o => o.HistoryRecords.Any(h => h.State == OrderStates.New
                    && (range.From == null || h.StateChangeDate >= range.From) && (range.To == null || h.StateChangeDate <= range.To)))
                .SelectMany(o => o.CartItems)
                .GroupBy(ci => ci.ProductId)
                .Join(_dbContext.ProductItems, g => g.Key, pi => pi.Id, (g, pi) => new { g, pi })
                .Select(gj => new
                {
                    ProductItem = gj.pi,
                    Count = gj.g.Count(),
                    Income = gj.g.Sum(ci => ci.ProductPrice),
                    Cost = gj.g.Sum(ci => ci.StorageItem.Price)
                })
                .Select(x => new ProductItemReportDTO()
                {
                    Id = x.ProductItem.Id,
                    Name = x.ProductItem.Name,
                    SoldCount =  x.Count,
                    Income = x.Income,
                    Cost = x.Cost,
                    Profit = x.Income - x.Cost,
                    AverageProfitPerItem = (x.Income - x.Cost) / x.Count
                });

            return Ok(result);
        }

        // GET: api/<controller>/total
        [HttpGet]
        [Route("total")]
        public async Task<IActionResult> GetProfits(DatesRangeModel range)
        {
            var records = _dbContext.Orders
                .AsNoTracking()
                .Where(o => o.HistoryRecords.Any(h => h.State == OrderStates.New
                    && (range.From == null || h.StateChangeDate >= range.From) && (range.To == null || h.StateChangeDate <= range.To)))
                .SelectMany(o => o.CartItems);

            var result = new TotalReportDTO()
            {
                SoldCount = await records.CountAsync(),
                Income = await records.SumAsync(r => r.ProductPrice),
                Cost = await records.SumAsync(r => r.StorageItem.Price),
            };
            result.Profit = result.Income - result.Cost;

            return Ok(result);
        }
		
		// GET: api/<controller>/dronesUtilization
		[HttpGet]
		[Route("dronesUtilization")]
		public async Task<IActionResult> GetDronesUtilization()
		{
			var records = _dbContext.DronePackingHistories
				.AsNoTracking()
				.GroupBy(x => x.OrderId)
				.Select(g => new
				{
					PerOrderUtilization = g.Sum(x => x.LoadedWeight) / g.Sum(x => x.MaxWeight)
				});

			var result = new DroneUtilizationReportDTO()
			{
				MinPerOrder = await records.MinAsync(x => x.PerOrderUtilization),
				MaxPerOrder = await records.MaxAsync(x => x.PerOrderUtilization),
				TotalAverage = await records.AverageAsync(x => x.PerOrderUtilization)
			};

			return Ok(result);
		}
	}
}
