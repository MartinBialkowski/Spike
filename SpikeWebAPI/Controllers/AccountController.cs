using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpikeConnectProviders.Abstract;
using SpikeConnectProviders.Extensions;
using SpikeWebAPI.DTOs;
using SpikeWebAPI.Extensions;

namespace SpikeWebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        public AccountController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = GetUser(userDTO.Email);
                return Ok(GenerateJwtToken(appUser));
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = userDTO.Email,
                Email = userDTO.Email
            };
            var result = await userManager.CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
            {
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, confirmationToken, Request.Scheme);
                await emailSender.SendConfirmationEmail(userDTO.Email, callbackUrl);
                await signInManager.SignInAsync(user, false);
                return Ok(GenerateJwtToken(user));
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("refresh")]
        public IActionResult RefreshToken()
        {
            string email = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var appUser = GetUser(email);

            return Ok(GenerateJwtToken(appUser));
        }


        [HttpGet("confirm")]
        [AllowAnonymous]
        public IActionResult ConfirmEmail()
        {
            throw new NotImplementedException();
        }

        // POST: /Account/LogOut
        // To do this, need to use Reference Token
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return NoContent();
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IdentityUser GetUser(string email)
        {
            return userManager.Users.SingleOrDefault(x => x.Email == email);
        }
    }
}
