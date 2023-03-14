using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using App_ChatApi.Data;
using App_ChatApi.ViewModels;
using App_ChatApi.Migrations;
using App_ChatApi.Responses;
using AutoMapper;
using App_ChatApi.Maps;
using App_ChatApi.Data.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App_ChatApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("add")]
        public async Task<RegistrationResponse> Register(RegisterViewModel model)
        {
            var response = new RegistrationResponse();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            var mapper = config.CreateMapper();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);
                var userDto = mapper.Map<ApplicationUserDto>(user);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    response.Success = true;
                    response.Message = "Succes .";
                    response.User = userDto;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error:";
                    response.Errors = result.Errors.Select(e => e.Description).ToList();
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Error:";
                response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            }

            return response;
        }

        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginViewModel model)
        {
            var response = new LoginResponse();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                   
                    var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
         
                        };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpiresInHours"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    var tokenName = "JwtToken";
                    var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "AppChat", tokenName);
                    if (existingToken != null)
                    {
                        await _userManager.RemoveAuthenticationTokenAsync(user, "AppChat", tokenName);
                    }
                    await _userManager.SetAuthenticationTokenAsync(user, "AppChat", tokenName, tokenString);
                    response.Token = tokenString;
                    response.Success = true;
                    response.Email = user.Email;
                    response.UserName = user.UserName;
                    response.Message = "Success";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    response.Success = false;

                    response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                response.Success = false;

                response.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            }
            return response;
        }

        //Cerrar sesión
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }

}
