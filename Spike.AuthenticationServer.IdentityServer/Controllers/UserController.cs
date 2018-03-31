using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<UserController> logger;

        public UserController
        (
            UserManager<IdentityUser> userManager,
            ILogger<UserController> logger
        )
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        // POST: api/users/{userId/claim
        [HttpPost("{userId}/claim")]
        public async Task<IActionResult> AssignClaim(string userId, [FromBody] ClaimDto claimDto)
        {
            logger.LogInformation($"POST: api/users/{0}/claim", userId);
            if (!ModelState.IsValid)
            {
                logger.LogWarning("User provided invalid data for request");
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning($"Cannot find user with provided id: {0}", userId);
                return NotFound($"User with provided id {userId} not exist");
            }

            var claim = new Claim(claimDto.Type, claimDto.Value);
            if (await IsClaimDuplicate(user, claim))
            {
                logger.LogWarning($"User {user.Id} already has claim with type {claim.ValueType} and value {claim.Value}");
                return StatusCode(StatusCodes.Status409Conflict);
            }

            var result = await userManager.AddClaimAsync(user, claim);
            if (result.Succeeded)
            {
				return NoContent();
			}

	        logger.LogError($"Cannot add claim: {result.Errors}");
	        return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

		// Delete api/users/{userId/claim
		[HttpDelete("{userid}/claim")]
        public async Task<IActionResult> RemoveClaim(string userId, [FromBody] ClaimDto claimDto)
        {
            logger.LogInformation($"DELETE: api/users/{0}/claim", userId);
            if (!ModelState.IsValid)
            {
                logger.LogWarning("User provided invalid data for request");
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning($"Cannot find user with provided id: {0}", userId);
                return NotFound($"User with provided id {userId} not exist");
            }

            var claim = new Claim(claimDto.Type, claimDto.Value);
            var result = await userManager.RemoveClaimAsync(user, claim);
            if (result.Succeeded)
            {
				return NoContent();
			}

	        logger.LogError($"Cannot add claim: {result.Errors}");
	        return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
			
        }

        private async Task<bool> IsClaimDuplicate(IdentityUser user, Claim claim)
        {
            var claims = await userManager.GetClaimsAsync(user);

            return claims.Any(x => x.Value == claim.Value);
        }
    }
}
