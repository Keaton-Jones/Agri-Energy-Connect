using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Agri_Energy_Connect.ViewModels;

namespace Agri_Energy_Connect.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly AECContext _context;

        public RegistrationController(ILogger<RegistrationController> logger, AECContext context)
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
        public async Task<IActionResult> Index(RegistrationViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Email == "" || !checkEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email is not valid.");
                }
                else if (user.Password == "" || !checkPassword(user.Password))
                {
                    ModelState.AddModelError("", "Password must be at least 6 characters long, contain at least one uppercase letter, one lowercase letter, and one digit.");
                }
                else if (user.Name == "" || !checkName(user.Name))
                {
                    ModelState.AddModelError("", "Name is required.");
                }
                else if (user.Surname == "" || !checkSurname(user.Surname))
                {
                    ModelState.AddModelError("", "Surname is required.");
                }
                else if (user.Phone == "" || !checkPhone(user.Phone))
                {
                    ModelState.AddModelError("", "Phone number is required.");
                }
                else if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                }
                else
                {
                    var userExists = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                    if (userExists != null)
                    {
                        ModelState.AddModelError("", "Email already exists. Please try again.");
                    }
                    else
                    {
                        var newUser = new User
                        {
                            Name = user.Name,
                            Surname = user.Surname,
                            Phone = user.Phone,
                            Email = user.Email,
                            Password = user.Password,
                            role = "User"
                        };
                        _context.Users.Add(newUser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(user);
        }

        public static bool checkPassword(string password)
        {
            // Check if the password is at least 6 characters long
            if (password.Length < 6)
            {
                return false;
            }

            // Check if the password contains at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check if the password contains at least one lowercase letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check if the password contains at least one digit
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // If all checks pass, the password is valid
            return true;
        }
        public static bool checkEmail(string email)
        {
            // Check if the email is in a valid format
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        public static bool checkPhone(string phone)
        {
            // Check if the phone number is in a valid format (10 digits)
            var phonePattern = @"^\d{10}$";
            return Regex.IsMatch(phone, phonePattern);
        }
        public static bool checkName(string name)
        {
            // Check if the name is in a valid format (only letters and spaces)
            var namePattern = @"^[a-zA-Z\s]+$";
            return Regex.IsMatch(name, namePattern);
        }
        public static bool checkSurname(string surname)
        {
            // Check if the surname is in a valid format (only letters and spaces)
            var surnamePattern = @"^[a-zA-Z\s]+$";
            return Regex.IsMatch(surname, surnamePattern);
        }
    }
}