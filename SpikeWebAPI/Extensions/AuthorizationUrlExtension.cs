using Microsoft.AspNetCore.Mvc;
using SpikeWebAPI.Controllers;

namespace SpikeWebAPI.Extensions
{
    public static class AuthorizationUrlExtension
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
