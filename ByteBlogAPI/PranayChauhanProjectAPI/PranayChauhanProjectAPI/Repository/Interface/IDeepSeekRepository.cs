namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface IDeepSeekRepository
    {
        Task<string> GetResponseAsync(string query);
    }
}
