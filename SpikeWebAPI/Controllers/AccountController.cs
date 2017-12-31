﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ContactProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        // POST: account/login
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

        // GET: /account/external-logins
        [HttpGet("external-logins")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogins()
        {
            return Ok(await signInManager.GetExternalAuthenticationSchemesAsync());
        }

        // POST: /account/login-microsoft
        [HttpGet("login-microsoft")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback");
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("callback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            { 
                return BadRequest(remoteError);
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Unauthorized();
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if(result.Succeeded)
            {

            }
            return Ok();
        }

        // POST: account/register
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

        // GET: account/refresh
        [HttpGet("refresh")]
        public IActionResult RefreshToken()
        {
            string email = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var appUser = GetUser(email);

            return Ok(GenerateJwtToken(appUser));
        }

        // GET: account/confirm
        [HttpGet("confirm")]
        [AllowAnonymous]
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

        // POST: /account/logout
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
