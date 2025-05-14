using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Agri_Energy_Connect.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.ViewModels;

namespace Agri_Energy_Connect.Controllers
{


    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AECContext _context;

        public LoginController(ILogger<LoginController> logger, AECContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {

                if (user.Email == "")
                {
                    ModelState.AddModelError("", "Email is required.");
                }
                else if (user.Password == "")
                {
                    ModelState.AddModelError("", "Password is required.");
                }
                else
                {
                    var userExists = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                    if (userExists != null)
                    {
                        HttpContext.Session.SetString("UserRole", userExists.role);
                        HttpContext.Session.SetInt32("UserId", userExists.UserId);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt. Please try again.");
            }
            return View(user);
        }

        

    }
}