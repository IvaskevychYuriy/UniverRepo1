using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Api.Constants;
using WebStore.Api.Models;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class DronesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DronesController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        // POST api/<controller>/
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DronesAddModel model)
        {
            for (int i = 0; i < model.Quantity; ++i)
            {
                await _dbContext.Drones.AddAsync(new Drone()
                {
                    State = DroneStates.Available,
                    ArrivalTime = null,
                    StorageId = model.StorageId,
                    CartItemId = null
                });
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var drone = await _dbContext.Drones.FindAsync(id);
            if (drone == null || drone.State != DroneStates.Available)
            {
                return BadRequest();
            }

            _dbContext.Drones.Remove(drone);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
