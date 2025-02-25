using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PranayChauhanProjectAPI.DTO;
using PranayChauhanProjectAPI.Repository.Implementation;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase


    {
        private readonly UserManager<IdentityUser> useManager;
        private readonly ITokenReoisitory tokenReoisitory;

        public AuthController(UserManager<IdentityUser> useManager,ITokenReoisitory tokenReoisitory)
        {
            this.useManager = useManager;
            this.tokenReoisitory = tokenReoisitory;
        }


        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            //Check Email   
            var identityUser = await useManager.FindByEmailAsync(request.Email);

            if (identityUser != null)
            {

                var checkPasswordResult = await useManager.CheckPasswordAsync(identityUser,request.Password);

                if (checkPasswordResult)
                {
                    //Create Token
                    var roles = await useManager.GetRolesAsync(identityUser);
                    var jwtToken = tokenReoisitory.CreateJwtToken(identityUser, roles.ToList());

                    var reponse = new LoginResponseDTO()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken,
                    };

                    return Ok(reponse);



                }
            }

                ModelState.AddModelError(" ", "Email or Password Incorrect");

            return ValidationProblem(ModelState);

            
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
            };


            var identityResult = await useManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {

                //Add User Role
                identityResult = await useManager.AddToRoleAsync(user, "Reader");
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {

                        foreach (var error in identityResult.Errors)
                        {

                            ModelState.AddModelError("", error.Description);
                        }


                    }

                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {

                    foreach (var error in identityResult.Errors)
                    {

                        ModelState.AddModelError("", error.Description);
                    }


                }

            }

            return ValidationProblem(ModelState);
        }
    }
}
