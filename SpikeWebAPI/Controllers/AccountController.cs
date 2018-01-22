using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Spike.Backend.Interface.Contact;
using Spike.WebApi.Extensions;
using Spike.WebApi.Types.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spike.WebApi.Controllers
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
        private readonly ILogger<AccountController> logger;

        public AccountController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            ILogger<AccountController> logger
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        // POST: account/login
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            logger.LogDebug("{@User} try to login", new { User = userDTO });
            if (!ModelState.IsValid)
            {
                logger.LogWarning("User sent invalid credentials");
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, false, false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(userDTO.Email);
                logger.LogInformation("User {userId} logged into application", new { userId = user.Id });
                return Ok(await GenerateJwtToken(user));
            }
            logger.LogWarning("User unable to login");
            return Unauthorized();
        }

        // POST: account/register
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            logger.LogDebug("{@User} try to login", new { User = userDTO });
            if (!ModelState.IsValid)
            {
                logger.LogWarning("User sent invalid credentials");
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
                logger.LogInformation("User {userId} registered successfully", new { userId = user.Id });
                await SendConfirmationEmail(user);
                await AssignBasicClaims(user);
                var jwt = await GenerateJwtToken(user);
                return Ok(jwt);
            }
            else
            {
                logger.LogWarning("Cannot register user");
                logger.LogDebug("Cannot register {@User}, {@errors}", new { User = user }, new { errors = result.Errors });
                return BadRequest(result.Errors);
            }
        }

        // GET: account/refresh
        [HttpGet("refresh")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await GetUserFromClaim();
            logger.LogInformation("user {userId}, prolonged token", new { userId = user.Id });
            return Ok(await GenerateJwtToken(user));
        }

        // GET: account/confirm
        [HttpGet("confirm")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> ConfirmEmail(AccountConfirmationDTO confirmationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByIdAsync(confirmationDTO.UserId);
            if (user == null)
            {
                return NotFound("User not exist");
            }
            var result = await userManager.ConfirmEmailAsync(user, confirmationDTO.Code);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return Forbid();
            }
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            await emailSender.SendResetPasswordEmail(user.Email, code);
            return Ok();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return Forbid();
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return Forbid();
            }
        }

        // POST: /account/logout
        // To do this, need to use Reference Token
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return NoContent();
        }

        private async Task SendConfirmationEmail(IdentityUser user)
        {
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, confirmationToken, Request.Scheme);
            await emailSender.SendConfirmationEmail(user.Email, callbackUrl);
            logger.LogDebug("Sent confiramtion email, to {address}", user.Email);
        }

        private async Task<IdentityResult> AssignBasicClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            return await userManager.AddClaimsAsync(user, claims);
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var claims = await PrepareClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                notBefore: DateTime.Now,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<IList<Claim>> PrepareClaims(IdentityUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var identifierClaim = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            claims.Insert(0, identifierClaim);

            return claims;
        }

        private async Task<IdentityUser> GetUserFromClaim()
        {
            string email = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return await userManager.FindByEmailAsync(email);
        }
    }
}
