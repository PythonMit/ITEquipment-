using ITEquipment.Interfaces;
using ITEquipment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITEquipment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public string _Constring;
        private IConfiguration _configuration;
        private readonly IAccount _account;
        
        public TokenController(IConfiguration configuration,IAccount account)
        {
            _configuration = configuration;
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _account = account;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> GetToken(LoginModel loginModel)
        {
            try
            {
                UserMasterViewModel loginViewModel = new UserMasterViewModel();
                loginViewModel = await _account.GetLoginUserDetail(loginModel.Email, loginModel.Password);
                if (loginViewModel != null && loginViewModel.Id > 0) { 
                    if(loginViewModel.Email==loginModel.Email && loginViewModel.Password == loginModel.Password)
                    {
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Name",loginViewModel.FirstName ),
                        new Claim("Email", loginViewModel.Email)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                       
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(50),
                            signingCredentials: signIn);
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
