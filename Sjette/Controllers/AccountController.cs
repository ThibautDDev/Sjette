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
    [Authorize] // Make sure that only authorized users can use methods of this controller. If not, the user gets redirected to the login with a returnurl to the requested page.
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


        /* 
         * Dictionairy that is used inside this controller to pass userinformation between functions.
         * This is an alternative for the User.Claims. Because of the internal use inside this controller,
         * the functions is set to private. 
        */
        private void setUserDictionairy()
        {
            foreach (var c in User.Claims)
            {
                UserDictionairy[c.Type.ToString()] = c.Value.ToString();
            }
        }


        private bool HasSpecialChars(string testString)
        {
            return testString.Any(c => !Char.IsLetterOrDigit(c));
        }


        private bool HasDigit(string testString) {
            return testString.Any(char.IsDigit);
        }


        /*
         * Function to hash a password with a given hash and non-hashed version of the password.
         * This function is private because of the internal use inside this controller.
        */
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

        
        /*
         * Function that returns an userObject of the SQL DB with a given context and email.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Users> getUserByEmailAsync(SjetteContext ctx, string email)
        {
            var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                               $"FROM Users AS U " +
                                               $"WHERE U.Email = '{email}'").ToListAsync();
            if (x.Count == 0) return null;
            return x.First();
        }


        /*
         * Function that returns an userObject of the SQL DB with a given context and id.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Users> getUserByIdAsync(SjetteContext ctx, int id)
        {
            return await ctx.Users.FindAsync(id);
        }


        /*
         * Function that returns an activityObject of the SQL DB with a given context and id.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Activities> getActivityByIdAsync(SjetteContext ctx, int id)
        {
            return await ctx.Activities.FindAsync(id);
        }


        /*
         * Function that returns an groupObject of the SQL DB with a given context and groupName.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Groups> getGroupByNameAsync(SjetteContext ctx, string groupName)
        {
            var x = await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                                $"FROM Groups AS G " +
                                                $"WHERE G.GroupName = '{groupName}'").ToListAsync();
            return x.First();
        }


        /* 
         * Function that returns a List of groupObjects of the SQL DB with a given context and userObject.
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<Groups>> getGroupsOfUserAsync(SjetteContext ctx, Users user)
        {
            return await ctx.Groups.FromSqlRaw($"SELECT G.* " +
                                               $"FROM Groups AS G " +
                                               $"INNER JOIN GroupMembership AS GM ON G.pk_GroupID = GM.GroupID " +
                                               $"INNER JOIN Users AS U ON GM.UserId = U.pk_UserID " +
                                               $"WHERE U.pk_UserID={user.pk_UserID}").ToListAsync();
        }


        /* 
         * Function that returns a List of activityObjects of the SQL DB with a given context and userObject 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<Activities>> getActivitiesOfUserAsync(SjetteContext ctx, Users user)
        {
            return await ctx.Activities.FromSqlRaw($"SELECT A.* " +
                                                   $"FROM Activities AS A " +
                                                   $"WHERE a.fk_UserID={user.pk_UserID} " +
                                                   $"AND A.StartTime <= GETDATE() " + 
                                                   $"ORDER BY StartTime DESC").ToListAsync();
        }


        /* 
         * Function that returns a Dictionary with as key the primaryKey of a group and as value all the activities
         * of all users of a group, with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Dictionary<int, List<Activities>>> getActivitiesOfGroupAsync(SjetteContext ctx, List<Groups> groupsOfUser)
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


        /* 
         * Function that returns a Dictionary with as key the primaryKey of a group and as value all the user
         * of a group, with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<Dictionary<int, List<Users>>> getAllUsersOfGroupAsync(SjetteContext ctx, List<Groups> groupsOfUser)
        {
            Dictionary<int, List<Users>> ReturnDict = new Dictionary<int, List<Users>>();
            foreach (Groups group in groupsOfUser)
            {
                var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                                   $"FROM Users AS U " +
                                                   $"WHERE U.pk_UserID IN " +
                                                       $"(SELECT G.UserID " +
                                                       $" FROM GroupMembership AS G " +
                                                       $" WHERE G.GroupID = {group.pk_GroupID})").ToListAsync();
                ReturnDict[group.pk_GroupID] = x;
            }
            return ReturnDict;
        }
        private static async Task<List<Users>> getAllUsersOfGroupAsync(SjetteContext ctx, Groups group)
        {
            var x = await ctx.Users.FromSqlRaw($"SELECT U.* " +
                                               $"FROM Users AS U " +
                                               $"WHERE U.pk_UserID IN " +
                                                   $"(SELECT G.UserID " +
                                                   $" FROM GroupMembership AS G " +
                                                   $" WHERE G.GroupID = {group.pk_GroupID})").ToListAsync();
            return x;
        }


        /* 
         * Function that returns a list of mutual users with a given context and list of groupObjects. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<MutualUsers>> getMutualUsers(SjetteContext ctx)
        {
            var x = await ctx.MutualUsers.FromSqlRaw($"SELECT ROW_NUMBER() OVER(ORDER BY GroupID ASC) AS Row, " +
                                                     $"U.*, GM.GroupID " + 
                                                     $"FROM Users AS U " +
                                                     $"INNER JOIN GroupMembership AS GM " +
                                                     $"ON U.pk_UserID = GM.UserID").ToListAsync();
            return x;
        }


        /* 
         * Function that returns a list of groupMembershipObjects with a given context and an userId. 
         * This functions is private because of the internal use inside this controller.
        */
        private static async Task<List<GroupMembership>> GetGroupMembershipsOfUserAsync(SjetteContext ctx, int userId)
        {
            var x = await ctx.GroupMembership.FromSqlRaw($"SELECT GM.* " +
                                                         $"FROM GroupMembership AS GM " +
                                                         $"WHERE GM.UserID={userId}").ToListAsync();
            return x;
        }



        // GET: Account/
        public async Task<IActionResult> Index()
        {
            setUserDictionairy();

            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var groups = await getGroupsOfUserAsync(_context, user);
            var activities = await getActivitiesOfUserAsync(_context, user);
            var groupActivities = await getActivitiesOfGroupAsync(_context, groups);

            DashboardData data = new DashboardData(user, groups, activities, groupActivities);

            return View(data);
        }


        // GET: Account/Activity/
        public async Task<IActionResult> Activity()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var activities = await getActivitiesOfUserAsync(_context, user);

            ActivityData data = new ActivityData(user,activities);

            return View(data);
        }


        // GET: Account/createActivity
        public IActionResult createActivity()
        {
            return View();
        }


        [HttpPost("createActivity")]
        public async Task<IActionResult> createNewActivity(string activityType, string activityName, string totalKms, DateTime startTime, TimeSpan totalTime, string gear, int calories)
        {
            if (totalKms.Contains(".")) totalKms = totalKms.Replace(".", ",");
            setUserDictionairy();

            Activities Activity = new Activities();
            Activity.fk_UserID = Convert.ToInt32(UserDictionairy["UserID"]);
            Activity.ActivityType = activityType;
            Activity.ActivityName = activityName;
            Activity.TKm = Convert.ToDecimal(totalKms);
            Activity.StartTime = startTime;
            Activity.TTime = totalTime;
            Activity.Gear = gear;
            Activity.TotalCalories = calories;

            _context.Add(Activity);
            await _context.SaveChangesAsync();
            TempData["Succes"] = "The activity has been succesfully created.";

            return Redirect("~/Account");
        }

        // POST: /editActivity
        [HttpPost("editActivity")]
        public async Task<IActionResult> editActivity(string redirectUrl, string activityId, string Name, DateTime Date, TimeSpan Duration, string Distance, int Calories, string Gear)
        {
            if (Distance.Contains(".")) Distance = Distance.Replace(".", ",");
            setUserDictionairy();

            Activities Activity = await getActivityByIdAsync(_context, Convert.ToInt32(activityId));
            Activity.ActivityName = Name;
            Activity.TKm = Convert.ToDecimal(Distance);
            Activity.StartTime = Date;
            Activity.TTime = Duration;
            Activity.Gear = Gear;
            Activity.TotalCalories = Calories;

            _context.Activities.Update(Activity);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "The activity-data has been succesfully updated.";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // POST: /deleteActivity
        [HttpPost("deleteActivity")]
        public async Task<IActionResult> deleteActivity(string redirectUrl, string activityId)
        {
            setUserDictionairy();
            Activities Activity = await getActivityByIdAsync(_context, Convert.ToInt32(activityId));

            _context.Activities.Remove(Activity);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "The activity has been succesfully deleted.";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // GET: Account/Group
        public async Task<IActionResult> Group()
        {
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var groups = await getGroupsOfUserAsync(_context, user);
            var activities = await getActivitiesOfUserAsync(_context, user);
            var groupActivities = await getActivitiesOfGroupAsync(_context, groups);
            var allUsersOfGroup = await getAllUsersOfGroupAsync(_context, groups);
            var mutualUsers = await getMutualUsers(_context);

            GroupData data = new GroupData(user, groups, activities, groupActivities, allUsersOfGroup, mutualUsers);

            return View(data);
        }


        // POST: createGroup
        [HttpPost("createGroup")]
        public async Task<IActionResult> createNewGroup(string redirectUrl, string name)
        {
            setUserDictionairy();
            int userId = Convert.ToInt32(UserDictionairy["UserID"]);

            Groups Group = new Groups();
            Group.fk_CreatorID = userId;
            Group.GroupName = name;
            _context.Add(Group);
            await _context.SaveChangesAsync();  // Or I don't have a primary key for the membership

            GroupMembership GroupMembership = new GroupMembership();
            GroupMembership.GroupID = Group.pk_GroupID;
            GroupMembership.UserID = userId;
            _context.Add(GroupMembership);
            await _context.SaveChangesAsync();

            TempData["Succes"] = "Group was succesfully created";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // POST: leaveGroup
        [HttpPost("leaveGroup")]
        public async Task<IActionResult> leaveGroup(string redirectUrl, string groupName)
        {
            bool done = false;
            setUserDictionairy();
            int userId = Convert.ToInt32(UserDictionairy["UserID"]);
            var group = await getGroupByNameAsync(_context, groupName);

            var MembershipList = await GetGroupMembershipsOfUserAsync(_context, userId);
            foreach (var item in MembershipList)
            {
                if (item.GroupID == group.pk_GroupID)
                {
                    _context.GroupMembership.Remove(item);
                    await _context.SaveChangesAsync();
                    done = true;
                }
            }

            if (done) TempData["Succes"] = "You succesfully left the group";
            else TempData["Error"] = "Something went wrong while leaving this group. Please contact an administrator";

            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // POST: deleteGroup
        [HttpPost("deleteGroup")]
        public async Task<IActionResult> deleteGroup(string redirectUrl, string groupName)
        {
            bool done = false;
            setUserDictionairy();
            int userId = Convert.ToInt32(UserDictionairy["UserID"]);
            var group = await getGroupByNameAsync(_context, groupName);
            var members = await getAllUsersOfGroupAsync(_context, group);

            //ExtraSecurityCheck - Request is from creator and group has only 1 member
            if (group.fk_CreatorID == userId && members.Count == 1)
            {
                var MembershipList = await GetGroupMembershipsOfUserAsync(_context, userId);
                var membership = MembershipList.Find(x => x.GroupID == group.pk_GroupID);

                if (membership != null)
                {
                    _context.GroupMembership.Remove(membership);
                    _context.Groups.Remove(group);
                    await _context.SaveChangesAsync();
                    done = true;
                } TempData["Error"] = "Something went wrong with deleting this group. Contact an admin.";
            } else return Redirect("~/Error/403");

            if (done) TempData["Succes"] = "Group was succesfully deleted";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // POST: addMember
        [HttpPost("addMember")]
        public async Task<IActionResult> addMember(string redirectUrl, string groupName, string email)
        {
            bool done = false;
            var group = await getGroupByNameAsync(_context, groupName);

            var newMember = await getUserByEmailAsync(_context, email);
            if (newMember != null)
            {
                var GroupMembershipsOfNewMember = await GetGroupMembershipsOfUserAsync(_context, newMember.pk_UserID);
                var isAlreadyMember = false;
                foreach (var item in GroupMembershipsOfNewMember) if (item.GroupID == group.pk_GroupID) isAlreadyMember = true;

                if (!isAlreadyMember)
                {
                    GroupMembership membership = new GroupMembership();
                    membership.GroupID = group.pk_GroupID;
                    membership.UserID = newMember.pk_UserID;

                    _context.GroupMembership.Add(membership);
                    await _context.SaveChangesAsync();
                    done = true;
                }
                else TempData["Error"] = "This email is linked to an user that already is a member of this group. Try again with a valid email";
            } else TempData["Error"] = "Email is not linked to a valid user. Try again with a valid email";

            if (done) TempData["Succes"] = "Member succesfully added to the group";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }


        // POST: removeMember
        [HttpPost("removeMember")]
        public async Task<IActionResult> removeMember(string redirectUrl, string groupName, int userToRemove)
        {
            bool done = false;
            setUserDictionairy();
            int userId = Convert.ToInt32(UserDictionairy["UserID"]);
            var group = await getGroupByNameAsync(_context, groupName);

            if (group.fk_CreatorID == userId)
            {
                var member = await getUserByIdAsync(_context, userToRemove);
                if (member != null)
                {
                    var MembershipList = await GetGroupMembershipsOfUserAsync(_context, member.pk_UserID);
                    var membership = MembershipList.Find(x => x.GroupID == group.pk_GroupID);
                    if (membership != null)
                    {
                        _context.GroupMembership.Remove(membership);
                        await _context.SaveChangesAsync();
                        done = true;
                    }
                    else TempData["Error"] = "This user is not a member of this group. Please select a valid member";
                }
                else TempData["Error"] = "This person is not a valid user of this application. Please select a valid member";
            }
            else redirectUrl = "/Error/403";

            if (done) TempData["Succes"] = "Member succesfully removed to the group";
            var url = $"~{redirectUrl}";
            return Redirect(url);
        }

        //TRANSFER ADMIN NOG DOEN
        // POST: removeMember
        [HttpPost("transferAdmin")]
        public async Task<IActionResult> transferAdmin(string redirectUrl, string groupName, int userToTransfer)
        {
            bool done = false;
            setUserDictionairy();
            int userId = Convert.ToInt32(UserDictionairy["UserID"]);
            var group = await getGroupByNameAsync(_context, groupName);

            if (group.fk_CreatorID == userId)
            {
                var member = await getUserByIdAsync(_context, userToTransfer);
                if (member != null)
                {
                    var MembershipList = await GetGroupMembershipsOfUserAsync(_context, member.pk_UserID);
                    var membership = MembershipList.Find(x => x.GroupID == group.pk_GroupID);
                    if (membership != null)
                    {
                        group.fk_CreatorID = member.pk_UserID;
                        _context.Groups.Update(group);
                        await _context.SaveChangesAsync();
                        done = true;
                    }
                    else TempData["Error"] = "This user is not a member of this group. Please select a valid member";
                }
                else TempData["Error"] = "This person is not a valid user of this application. Please select a valid member";
            }
            else redirectUrl = "/Error/403";

            if (done) TempData["Succes"] = "Admin-rights were succesfully transfered";
            var url = $"~{redirectUrl}";
            return Redirect(url);
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
            bool done = false;
            setUserDictionairy();
            var id = Convert.ToInt32(UserDictionairy["UserID"]);
            var user = await getUserByIdAsync(_context, id);
            var oldPasswordHash = (oldPassword != null) ? hashPassword(user.Hash, oldPassword):"";

            if (firstName != null)
            {
                user.FirstName = firstName;
                done = true;
            }
            if (lastName != null)
            {
                user.LastName = lastName;
                done = true;
            }
            if (email != null)
            {
                user.Email = email;
                done = true;
            }

            if (user.PasswordHash == oldPasswordHash && newPassword == newPasswordConfirm && newPassword != null)
            {
                if (HasSpecialChars(newPassword) && newPassword.Length >= 7 && HasDigit(newPassword))
                {
                    user.PasswordHash = hashPassword(user.Hash, newPassword);
                    done = true;
                }
                else TempData["PasswordRequirementsError"] = "Please make sure that the new password meets the requirements.";

            } else if (newPassword != newPasswordConfirm)
            {
                TempData["NewPasswordError"] = "Password did not match with each other.";
            } else if (newPassword == "")
            {
                TempData["NewPasswordError"] = "Please fill in a password";
            } else
            {
                TempData["PasswordError"] = "Old Password did not match with this account.";
            }

            if (done)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                TempData["Succes"] = "Settings were succesfully saved";
                
            }

            if (done && TempData["PasswordError"] == null && TempData["NewPasswordError"] == null && TempData["PasswordRequirementsError"] == null)
            {
                return Redirect("~/Account");
            } 
            else return Redirect("~/Account/Settings");
        }



        // GET: /logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }

    }
}
