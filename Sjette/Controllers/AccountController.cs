using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sjette.Models;
using Sjette.Models.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sjette.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SjetteContext _context;
        private Dictionary<string, string> UserDictionairy = new Dictionary<string, string>();


        public AccountController(ILogger<AccountController> logger, SjetteContext context)
        {
            _logger = logger;
            _context = context;
        }


        private void setUserDictionairy()
        {
            foreach (var c in User.Claims)
            {
                UserDictionairy[c.Type.ToString()] = c.Value.ToString();
            }
        }

        private string hashPassword(string hash, string password)
        {
            byte[] salt = Convert.FromBase64String(hash);

            string returnPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return returnPassword;
        } 


        private static async Task<Users> getUserByIdAsync(SjetteContext ctx, int id)
        {
            return await ctx.Users.FindAsync(id);
        }


        private static async Task<List<Groups>> getGroupsOfUserAsync(SjetteContext ctx, Users user)
        {
            return await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                               $"FROM Groups AS G " +
                                               $"INNER JOIN GroupMembership AS GM ON G.pk_GroupID = GM.GroupID " +
                                               $"INNER JOIN Users AS U ON GM.UserId = U.pk_UserID " +
                                               $"WHERE U.pk_UserID={user.pk_UserID}").ToListAsync();
        }


        private static async Task<List<Activities>> getActivitiesOfUserAsync(SjetteContext ctx, Users user)
        {
            return await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                   $"FROM Activities as A " +
                                                   $"WHERE A.fk_UserID={user.pk_UserID} " +
                                                   $"AND A.StartTime <= GETDATE() " + 
                                                   $"ORDER BY StartTime DESC").ToListAsync();
        }


        private static async Task<Dictionary<int, List<Activities>>> getActivitiesOfGroup(SjetteContext ctx, List<Groups> groupsOfUser)
        {
            Dictionary<int, List <Activities>> ReturnDict = new Dictionary<int, List<Activities>>();
            foreach (Groups group in groupsOfUser)
            {
                var x = await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                        $"FROM Activities AS A " +
                                                        $"WHERE a.fk_UserID in " +
                                                            $"(SELECT G.UserID " +
                                                            $"FROM GroupMembership AS G " +
                                                            $"WHERE G.GroupID = {group.pk_GroupID}) " +
                                                        $"AND A.StartTime <= GETDATE() " +
                                                        $"ORDER BY StartTime DESC").ToListAsync();
                ReturnDict[group.pk_GroupID] = x;
            }
            return ReturnDict;
        }


        // GET: Account/
        public async Task<IActionResult> Index()
        {
            setUserDictionairy();

            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var groups = await getGroupsOfUserAsync(_context, user);
            var activities = await getActivitiesOfUserAsync(_context, user);
            var groupActivites = await getActivitiesOfGroup(_context, groups);

            return View(Tuple.Create(user, groups, activities, groupActivites));
        }


        // GET: Account/Activity/
        public IActionResult Activity()
        {
            return View();
        }


        // GET: Account/createActivity
        public IActionResult createActivity()
        {

            return View();
        }


        [HttpPost("createActivity")]
        public async Task<IActionResult> createNewActivity(string activityType, string activityName, decimal totalKms, DateTime startTime, TimeSpan totalTime, string gear, int calories)
        {
            setUserDictionairy();

            Activities Activity = new Activities();
            Activity.fk_UserID = Convert.ToInt32(UserDictionairy["UserID"]);
            Activity.ActivityType = activityType;
            Activity.ActivityName = activityName;
            Activity.TKm = totalKms;
            Activity.StartTime = startTime;
            Activity.TTime = totalTime;
            Activity.Gear = gear;
            Activity.TotalCalories = calories;


            _context.Add(Activity);
            await _context.SaveChangesAsync();
            return Redirect("~/Account");
        }


        // GET: Account/EditActivity
        public IActionResult EditActivity()
        {
            return View();
        }


        // GET: Account/DeleteActivity
        public IActionResult DeleteActivity()
        {
            return View();
        }


        // GET: Account/Group
        public IActionResult Group()
        {
            return View();
        }


        // GET: Account/CreateGroup
        public IActionResult CreateGroup()
        {
            return View();
        }


        // GET: Account/LeaveGroup
        public IActionResult LeaveGroup()
        {
            return View();
        }


        // GET: Account/DeleteGroup
        public IActionResult DeleteGroup()
        {
            return View();
        }


        // GET: Account/Settings/
        public async Task<IActionResult> Settings()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            return View(user);
        }

        // POST: /saveSettings
        [HttpPost("saveSettings")]
        public async Task<IActionResult> SaveSettings(string firstName, string lastName, string email, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var oldPasswordHash = hashPassword(user.Hash, oldPassword);
            if (user.PasswordHash == oldPasswordHash && newPassword == newPasswordConfirm)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.PasswordHash = hashPassword(user.Hash, newPassword);
                await _context.SaveChangesAsync();
                return Redirect("/Account");
            } else if (newPassword != newPasswordConfirm)
            {
                TempData["NewPasswordError"] = "Password did not match with each other.";
            } else
            {
                TempData["PasswordError"] = "Old Password did not match with this account.";
            }
            return View("Settings");
        }



        // GET: /logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }

    }
}
