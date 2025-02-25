using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PranayChauhanProjectAPI.Configuration;
using PranayChauhanProjectAPI.Repository.Interface;

public class ChatGPTRepository : IChatGPTRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatGPTRepository(HttpClient httpClient, IOptions<OpenAISettings> openAISettings)
    {
        _httpClient = httpClient;
        _apiKey = openAISettings.Value.ApiKey;
    }

    public async Task<string> GetChatGPTResponseAsync(string prompt)
    {
        var request = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
            new { role = "system", content = "You are an AI assistant." },
            new { role = "user", content = prompt }
        }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
        var responseString = await response.Content.ReadAsStringAsync();

        // Log the raw response for debugging
        Console.WriteLine($"API Response: {responseString}");

        if (!response.IsSuccessStatusCode)
        {
            return $"Error: API call failed with status code {response.StatusCode}. Response: {responseString}";
        }

        try
        {
            using var doc = JsonDocument.Parse(responseString);

            // ✅ Check if "choices" exists in the response
            if (!doc.RootElement.TryGetProperty("choices", out var choicesArray) || choicesArray.GetArrayLength() == 0)
            {
                return "Error: No response from OpenAI API. Please check the API key and request.";
            }

            return choicesArray[0].GetProperty("message").GetProperty("content").GetString();
        }
        catch (JsonException ex)
        {
            return $"Error parsing response: {ex.Message}\nResponse: {responseString}";
        }
    }


}
