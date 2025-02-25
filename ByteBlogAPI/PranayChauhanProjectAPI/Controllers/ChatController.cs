using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PranayChauhanProjectAPI.DTO;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
         private readonly IChatGPTRepository _chatRepository;
    
        public ChatController(IChatGPTRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }



        [HttpPost]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequestDTO request)
        {

            if (request == null || string.IsNullOrEmpty(request.Prompt)) {

                return BadRequest("Prompt is requried");
            }

            var responseText = await _chatRepository.GetChatGPTResponseAsync(request.Prompt);
            var responseDTO = new ChatResponseDTO { Response = responseText };

            return Ok(responseDTO);

        }
    }
}
