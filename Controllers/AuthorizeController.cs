using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
namespace JWTAuthSample.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private JwtSettings _jwtSetting;
        public AuthorizeController(IOptions<JwtSettings> _jwtSettingsAccesser)
        {
            _jwtSetting= _jwtSettingsAccesser.Value;
        }
        [HttpPost]
        public IActionResult Token([FromBody]LoginViewModel viewModel)
        {
            if(ModelState.IsValid )
            {
                if(!(viewModel.User =="best" && viewModel.Password =="123"))
                {
                    return BadRequest();
                }
                var claims =new List<Claim>{
                    new Claim(ClaimTypes.Name,"zhangshuai"),
                    new Claim(ClaimTypes.Role,"admin")

                };
                var key =new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSetting.SecretKey));
                var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                var token =new JwtSecurityToken(
                    _jwtSetting.Issuer,
                    _jwtSetting.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds
                );
                return Ok(new{token =new JwtSecurityTokenHandler().WriteToken(token)});
            }
            else
            {
                return BadRequest();
            }


            //return Ok();
        }
    }
}
