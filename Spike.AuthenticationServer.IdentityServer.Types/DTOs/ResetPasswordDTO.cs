﻿namespace Spike.AuthenticationServer.IdentityServer.Types.DTOs
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
