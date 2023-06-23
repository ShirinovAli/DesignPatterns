using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StrategyPattern.WebApp.Models;
using System.Security.Claims;

namespace StrategyPattern.WebApp.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();
            var claim = User.Claims.FirstOrDefault(x => x.Type == Settings.ClaimDatabaseType)?.Value;
            if (claim != null)
            {
                settings.DatabaseType = (DatabaseType)int.Parse(User.Claims.FirstOrDefault(x => x.Type == Settings.ClaimDatabaseType).Value);
            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeDatabase(int DatabaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var newClaim = new Claim(Settings.ClaimDatabaseType, DatabaseType.ToString());

            var userClaims = await _userManager.GetClaimsAsync(user);

            var hasDatabaseTypeClaim = userClaims.FirstOrDefault(x => x.Type == Settings.ClaimDatabaseType);
            if (hasDatabaseTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            await _signInManager.SignOutAsync();

            var authenticateResult = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, authenticateResult.Properties);

            return RedirectToAction("Index");
        }
    }
}
