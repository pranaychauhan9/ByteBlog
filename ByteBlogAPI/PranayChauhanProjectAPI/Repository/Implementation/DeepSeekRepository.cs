using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PranayChauhanProjectAPI.Configuration;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Repository
{
    public class DeepSeekRepository : IDeepSeekRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public DeepSeekRepository(IOptions<DeepSeekSettings> openAISettings)
        {
            _httpClient = new HttpClient();
            _apiKey = openAISettings.Value.ApiKey;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetResponseAsync(string query)
        {
            var requestUrl = "https://api-inference.huggingface.co/models/gpt2"; // Replace with your desired model
            var requestBody = new
            {
                inputs = query
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                throw new Exception($"Error calling Hugging Face API: {response.StatusCode}");
            }
        }
    }
}