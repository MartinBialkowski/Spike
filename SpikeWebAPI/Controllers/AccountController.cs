﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = userManager.Users.SingleOrDefault(r => r.Email == userDTO.Email);
                return Ok(GenerateJwtToken(userDTO.Email, appUser));
            }
            return Unauthorized();
        }

        [HttpPost("register")]
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
                await signInManager.SignInAsync(user, false);
                return Ok(GenerateJwtToken(userDTO.Email, user));
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private string GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}