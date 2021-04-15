using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Sjette.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sjette.Models.Data;

namespace Sjette.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SjetteContext _context;

        public HomeController(ILogger<HomeController> logger, SjetteContext context)
        {
            _logger = logger;
            _context = context;
        }

        private bool HasSpecialChars(string testString)
        {
            return testString.Any(c => !Char.IsLetterOrDigit(c));
        }


        private bool HasDigit(string testString)
        {
            return testString.Any(char.IsDigit);
        }


        // GET: Home/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Register
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        // GET: /Login
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //POST: /register
        [HttpPost("register")]
        public async Task<IActionResult> registerUser(string firstName, string lastName, string email, string nonHashedPassword, string confirm_nonHashedPassword)
        {
            if (nonHashedPassword != confirm_nonHashedPassword)
            {
                TempData["PasswordError"] = "Password's don't match.";
                return View("Register");
            }

            if (!HasDigit(nonHashedPassword) || !HasSpecialChars(nonHashedPassword))
            {
                TempData["PasswordReqError"] = "Make sure that the password meets the requirements.";
                return View("Register");
            }

            var UserList = await _context.Users.FromSqlRaw($"SELECT * FROM Users Where Email='{email}'").ToListAsync();
            if (UserList.Count != 0)
            {
                TempData["EmailError"] = "Email is already in use. Try to login instead.";
                return View("Register");
            }

            // Hash password
            var salt = new byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            var saltString = Convert.ToBase64String(salt);

            string password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: nonHashedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            Users user = new Users();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.PasswordHash = password;
            user.Hash = saltString;
            user.CreationDate = DateTime.Now;


            _context.Add(user);
            await _context.SaveChangesAsync();

            await validate(email, nonHashedPassword, "~/");
            return Redirect("~/Account");
        }

        // POST: /login
        // Authentication part!
        [HttpPost("login")]
        public async Task<IActionResult> validate(string email, string nonHashedPassword, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var UserList = await _context.Users.FromSqlRaw($"SELECT * FROM Users Where Email='{email}'").ToListAsync();
            if (UserList.Count != 0)
            {
                var SqlUser = UserList[0];

                byte[] salt = Convert.FromBase64String(SqlUser.Hash);

                string password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: nonHashedPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));


                if (email == SqlUser.Email && password == SqlUser.PasswordHash)
                {
                    var claims = new List<Claim>();
                    //claims is de user en we voegen alles toe van info die we bijhouden in de cookie

                    claims.Add(new Claim("UserID", SqlUser.pk_UserID.ToString()));
                    claims.Add(new Claim("FirstName", SqlUser.FirstName));
                    claims.Add(new Claim("LastName", SqlUser.LastName));
                    claims.Add(new Claim("Email", email));
                    if (SqlUser.Admin == true)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity); // Authticket die we doorgeven aan SignInAsync functie
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect(returnUrl);
                }

                else
                {
                    TempData["PasswordError"] = "Password did not match with this account.";
                }
            }
            else
            {
                TempData["EmailError"] = "We didn't recognize this email."; //Custom errors en classes eraan verbinden
            }
            return View("login");
        }


        // GET: /denied
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();

        }
    }
}
