using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class UselessController : Controller
    {
        public UselessController()
        {
        }

        // GET: /Useless/Get?id=
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var result = UselessTaskManager.Get(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // POST: /Useless/New?workAmount=
        [AllowAnonymous]
        [HttpPost("New")]
        public async Task<IActionResult> New(long workAmount)
        {
            if (workAmount < 1 || workAmount > 1000000)
            {
                return BadRequest();
            }

            var result = UselessTaskManager.StartNew(workAmount);
            return Ok(result);
        }

        // POST: /Useless/Cancel?id=
        [AllowAnonymous]
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await UselessTaskManager.Stop(id);
            return Ok();
        }

        // GET: /Useless/Progress?id=
        [AllowAnonymous]
        [HttpGet("Progress")]
        public async Task<IActionResult> Progress(int id)
        {
            long progress = UselessTaskManager.ReportProgress(id);
            return Ok(progress);
        }
    }
}
