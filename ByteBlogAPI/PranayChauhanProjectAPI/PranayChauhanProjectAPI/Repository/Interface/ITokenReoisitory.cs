using Microsoft.AspNetCore.Identity;

namespace PranayChauhanProjectAPI.Repository.Interface
{
    public interface ITokenReoisitory
    {

        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
