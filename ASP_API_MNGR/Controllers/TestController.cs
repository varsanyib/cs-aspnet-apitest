using Microsoft.AspNetCore.Mvc;

namespace ASP_API_MNGR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private Random rnd = new Random(((int)DateTime.Now.Ticks));

        [HttpGet]
        public int GetRandom()
        {
            return rnd.Next();
        }
    }
}
