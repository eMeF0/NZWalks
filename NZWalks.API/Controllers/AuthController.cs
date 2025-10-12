using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        //Post: api/Auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

           var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);
           if (identityResult.Succeeded)
           {
                //Add roles to user here
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered!");
                    }
                } 

           }

           return BadRequest("User could not be registered");
        }

        //Post: api/Auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (password)
                {
                    //get roles
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        //Create token
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(jwtToken);
                    }
                }
            }

            return BadRequest("Invalid username or password");
        }
    }
}
