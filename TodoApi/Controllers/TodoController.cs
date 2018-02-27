using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Constants;

namespace TodoApi.Controllers
{
    [Route(Routes.TodoRoute)]
    public class TodoController : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}