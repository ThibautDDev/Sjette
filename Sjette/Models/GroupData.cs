using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sjette.Models
{
    // DashboardData model to pass multiple models in 1 combined one.
    // Also used to combine all razor code and variables.
    // Not the most efficient but cleanest way to code.
    public class GroupData
    {
        // ObjectProperties
        public Users User { get; set; }
        public List<Groups> UserGroups { get; set; }
        public Dictionary<int, List<Users>> AllUsersOfGroup { get; set; }
        public List<Activities> UserActivities { get; set; }
        public Dictionary<int, List<Activities>> ActivitiesOfAllGroups { get; set; }


        // GroupStats
        public List<Groups> GroupsCreated { get; set; }
        public List<Users> MostMutualGroups { get; set; }


        // Constructor
        public GroupData(Users user, List<Groups> groups, List<Activities> activities, 
            Dictionary<int, List<Activities>> groupActivities, 
            Dictionary<int, List<Users>>  allUsersOfGroup, List<Users> mutualUsers)
        {
            this.User = user;
            this.UserActivities = activities;
            this.UserGroups = groups;
            this.ActivitiesOfAllGroups = groupActivities;
            this.AllUsersOfGroup = allUsersOfGroup;

            setGroupsCreated();
            setMostMutualGroups(mutualUsers);
            //System.Diagnostics.Debugger.Break();
        }


        private void setGroupsCreated()
        {
            var listBeforeReturn = new List<Groups>();
            foreach (var group in UserGroups) if (group.fk_CreatorID == User.pk_UserID) listBeforeReturn.Add(group);
            this.GroupsCreated = listBeforeReturn;
        }



        private void setMostMutualGroups(List<Users> mutualUsers)
        {
            var returnLst = new List<Users>();
            var dictBeforeReturn = new Dictionary<Users, int>();
            foreach (var user in mutualUsers)
            {
                if(user != User)
                {
                    int prevValue;
                    dictBeforeReturn.TryGetValue(user, out prevValue);
                    dictBeforeReturn[user] = prevValue + 1;
                }
                
            }

            var dictBeforeReturnOrdered = dictBeforeReturn.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var maxValue = dictBeforeReturnOrdered.FirstOrDefault().Value;
            foreach (var item in dictBeforeReturnOrdered) if (item.Value >= maxValue) returnLst.Add(item.Key);

            this.MostMutualGroups = returnLst;
        }


    }
}
