namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface IChatGPTRepository
    {

        Task<string> GetChatGPTResponseAsync(string prompt);
    }
}
