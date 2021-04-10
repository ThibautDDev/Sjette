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


        // LineCharts
        public Dictionary<string, Dictionary<string, List<int>>> LineChartData { get; set; }


        // Table
        public Dictionary<string, List<string>> TableHeaders {get; set;}                                         //Activity -  Headers
        public Dictionary<int, Dictionary<string, List<Tuple<string, List<int>>> >> TableData { get; set; }      //GroupId  -  Dict<Activity - List(UserName - List<numbers>>)

        // Constructor
        public GroupData(Users user, List<Groups> groups, List<Activities> activities, 
            Dictionary<int, List<Activities>> groupActivities, 
            Dictionary<int, List<Users>>  allUsersOfGroup, List<Users> mutualUsers)
        {
            string[] array1 = { "Activities", "Hiking", "Cycling", "Running", "Calories", "Distance"};
            List<string> listOfLineChartProperties = new List<string>(array1);

            this.User = user;
            this.UserActivities = activities;
            this.UserGroups = groups;
            this.ActivitiesOfAllGroups = groupActivities;
            this.AllUsersOfGroup = allUsersOfGroup;

            setGroupsCreated();
            setMostMutualGroups(mutualUsers);

            SetLineChartData(listOfLineChartProperties);

            SetTableHeaders();
            SetTableData();
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


        // Set data of an user for the lineChart
        private void SetLineChartData(List<string> properties)
        {
            var tempDictBeforeReturn = new Dictionary<string, Dictionary<string, List<int>>>();
            foreach (var group in UserGroups)
            {
                var tempDictionary = new Dictionary<string, List<int>>();
                foreach (var property in properties)
                {
                    var lst = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    foreach (var activity in ActivitiesOfAllGroups[group.pk_GroupID]) 
                    {
                        var x = (DateTime.Now - activity.StartTime).TotalDays;
                        if (x < 365 && x > 0)
                        {
                            var month = activity.StartTime.Month - 1;
                            var prev = lst[month];
                            if (property == "Activities" || activity.ActivityType == property) lst[month] = prev + 1;
                            if (property == "Calories") lst[month] = prev + activity.TotalCalories;
                            if (property == "Distance") lst[month] = prev + Convert.ToInt32(activity.TKm);
                        }
                        
                    }
                tempDictionary[property] = lst;
                }
            tempDictBeforeReturn[group.GroupName] = tempDictionary;
            }
            this.LineChartData = tempDictBeforeReturn;
        }


        private void SetTableHeaders()
        {
            string[] array1 = { "Name", "Amount", "Longest Ride", "Total Distance", "Total Calories", "Fastest 50km", "Fastest 100km"};
            string[] array2 = { "Name", "Amount", "Longest Run", "Total Distance", "Total Calories", "Fastest 5km", "Fastest 10km", "Fastest 21.1km", "Fastest 42.2km" };
            string[] array3 = { "Name", "Amount", "Hikes 10km+", "Hikes 25km+", "Hikes 100km+", "Total Distance", "Total Calories"};

            var tempDictBeforeReturn = new Dictionary<string, List<string>>();
            var list1 = new List<string>(array1);
            var list2 = new List<string>(array2);
            var list3 = new List<string>(array3);

            tempDictBeforeReturn["Cycling"] = list1;
            tempDictBeforeReturn["Running"] = list2;
            tempDictBeforeReturn["Hiking"] = list3;

            this.TableHeaders = tempDictBeforeReturn;
        }


        private void SetTableData()
        {
            var tempDictBeforeReturn = new Dictionary<int, Dictionary<string, List<Tuple<string, List<int>>> >>();
            foreach (var item in ActivitiesOfAllGroups)
            {

            }
        }

    }
}
