using Microsoft.AspNetCore.Mvc;
using PranayChauhanProjectAPI.Repository.Interface;
using System.Threading.Tasks;

namespace PranayChauhanProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeepSeekController : ControllerBase
    {
        private readonly IDeepSeekRepository _deepSeekRepository;

        public DeepSeekController(IDeepSeekRepository deepSeekRepository)
        {
            _deepSeekRepository = deepSeekRepository;
        }

        [HttpPost("query")]
        public async Task<IActionResult> GetResponse([FromBody] string query)
        {
            try
            {
                // Call the repository to get the response from GPT4All
                var response = await _deepSeekRepository.GetResponseAsync(query);

                // Return the response as JSON
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 status code
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}