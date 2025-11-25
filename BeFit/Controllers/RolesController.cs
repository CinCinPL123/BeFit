using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BeFit.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BeFit.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RolesController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> AssignAdult()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Adult");
            
            if (!usersInRole.Any())
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    await _userManager.AddToRoleAsync(currentUser, "Adult");
                    return Content("Rola Adult została przypisana do Twojego konta.");
                }
            }
            
            return Content("Rola Adult została już przypisana do innego użytkownika.");
        }
    }
}
