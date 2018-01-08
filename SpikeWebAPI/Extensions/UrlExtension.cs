using Microsoft.AspNetCore.Mvc;
using Spike.WebApi.Controllers;

namespace Spike.WebApi.Extensions
{
    public static class UrlExtension
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
