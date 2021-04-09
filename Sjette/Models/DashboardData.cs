using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sjette.Models
{
    // DashboardData model to pass multiple models in 1 combined one.
    // Also used to combine all razor code and variables.
    // Not the most efficient but cleanest way to code.
    public class DashboardData
    {
        // ObjectProperties
        public List<Activities> UserActivities { get; set; }
        public Users User { get; set; }
        public List<Groups> UserGroups { get; set; }
        public Dictionary<int, List<Activities>> ActivitiesOfAllGroups { get; set; }

        // TotalCaloriesProperty
        public int TotalCalories { get; set; }

        // ActivitiesArrayProperties
        public List<Activities> ActivitiesCycling { get; set; }
        public List<Activities> ActivitiesRunning { get; set; }
        public List<Activities> ActivitiesHiking { get; set; }
        public List<Activities> ActivitiesLastMonth { get; set; }

        // GroupRankingProperties
        public Dictionary<int, Dictionary<int, Dictionary<string, int>>> GroupRankingData { get; set; }
        public Dictionary<string, Dictionary<int, List<int>>> GroupRankingByProperty { get; set; }

        // LineChartDataProperty
        public Dictionary<string, List<int>> LineChartData { get; set; }


        // Constructor
        public DashboardData(Users user, List<Groups> groups, List<Activities> activities, Dictionary<int, List<Activities>> groupActivities)
        {
            string[] array1 = { "Activities", "Hiking", "Cycling", "Running", "Distance", "Calories" };
            List<string> listOfGroupRankingProperties = new List<string>(array1);

            string[] array2 = { "Activities", "Hiking", "Cycling", "Running"};
            List<string> listOfLineChartProperties = new List<string>(array2);

            this.User = user;
            this.UserActivities = activities;
            this.UserGroups = groups;
            this.ActivitiesOfAllGroups = groupActivities;

            SetTotalCalories();

            SetActivitiesOfLastMonth();
            SetActivityTypes();

            SetGroupRankingData();
            SetGroupRankingByProperty(listOfGroupRankingProperties);

            SetLineChartData(listOfLineChartProperties);
            //System.Diagnostics.Debugger.Break();
        }


        // Calculate and set total calories of an user
        private void SetTotalCalories()
        {
            int totalCalories = 0;
            foreach (var item in UserActivities) totalCalories += item.TotalCalories;
            this.TotalCalories = totalCalories;
        }


        // Set activities of last month of an user
        private void SetActivitiesOfLastMonth()
        {
            var lstActivitiesLastMonth = new List<Activities>();
            foreach (var item in UserActivities)
            {
                var x = (DateTime.Now - item.StartTime).TotalDays;
                if (x < 30 && x > 0) lstActivitiesLastMonth.Add(item);
            }
            this.ActivitiesLastMonth = lstActivitiesLastMonth;
        }


        // Sort and set activities by type
        private void SetActivityTypes()
        {
            var lstCycling = new List<Activities>();
            var lstHiking = new List<Activities>();
            var lstRunning = new List<Activities>();
            foreach (var item in UserActivities)
            {
                if (item.ActivityType == "Cycling") lstCycling.Add(item);
                else if (item.ActivityType == "Hiking") lstHiking.Add(item);
                else if (item.ActivityType == "Running") lstRunning.Add(item);
            }
            this.ActivitiesCycling = lstCycling;
            this.ActivitiesHiking = lstHiking;
            this.ActivitiesRunning = lstRunning;
        }


        // Set ranking of the groups of an user
        private void SetGroupRankingData()
        {
            var rankingGroups = new Dictionary<int, Dictionary<int, Dictionary<string, int>>>();
            foreach (var i in ActivitiesOfAllGroups)
            {
                int groupId = i.Key;
                var activityList = i.Value;
                var userDict = new Dictionary<int, Dictionary<string, int>>();

                foreach (var activity in activityList)
                {
                    var userId = activity.fk_UserID;
                    var activityProp = new Dictionary<string, int>();
                    var keylist = new List<int>(userDict.Keys);
                    if (keylist.Contains(userId))
                    {
                        int currentCount;
                        activityProp = userDict[userId];

                        activityProp.TryGetValue(activity.ActivityType, out currentCount);
                        activityProp[activity.ActivityType] = currentCount + 1;

                        activityProp.TryGetValue("Calories", out currentCount);
                        activityProp["Calories"] = currentCount + activity.TotalCalories;

                        activityProp.TryGetValue("Distance", out currentCount);
                        activityProp["Distance"] = currentCount + Convert.ToInt32(activity.TKm);
                    }
                    else
                    {
                        activityProp["Running"] = 0;
                        activityProp["Cycling"] = 0;
                        activityProp["Hiking"] = 0;

                        activityProp[activity.ActivityType] = 1;
                        activityProp["Calories"] = activity.TotalCalories;
                        activityProp["Distance"] = Convert.ToInt32(activity.TKm);
                    }
                    userDict[userId] = activityProp;
                }
                rankingGroups[groupId] = userDict;
            }
            this.GroupRankingData = rankingGroups;
        }


        // Set ranking of an user in a group by a property
        private void SetGroupRankingByProperty(List<string> properties)
        {
            var tempDictBeforeReturn = new Dictionary<string, Dictionary<int, List<int>>>();
            foreach (var property in properties)
            {
                var rankingDictionary = new Dictionary<int, List<int>>();
                foreach (var i in GroupRankingData)
                {
                    var groupId = i.Key;
                    var userDict = i.Value;
                    var tempRankingDict = new Dictionary<int, int>();
                    foreach (var j in userDict)
                    {    
                        if (property == "Activities") tempRankingDict[j.Key] = (j.Value["Hiking"] + j.Value["Cycling"] + j.Value["Running"]);
                        else tempRankingDict[j.Key] = j.Value[property];
                    }

                    var sortedKeys = tempRankingDict.OrderByDescending(x => x.Value).Select(kvp => kvp.Key).ToList();
                    var index = sortedKeys.IndexOf(User.pk_UserID);
                    rankingDictionary[groupId] = new List<int>(new int[] { index + 1, tempRankingDict.Count, 100 * (tempRankingDict.Count - index - 1) / (tempRankingDict.Count - 1) });
                }
                tempDictBeforeReturn[property] = rankingDictionary;
            }
            this.GroupRankingByProperty = tempDictBeforeReturn;
        }


        // Set data of an user for the lineChart
        private void SetLineChartData(List<string> properties)
        {
            var tempDictBeforeReturn = new Dictionary<string, List<int>>();
            foreach(var property in properties)
            {
                var chartArray = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                foreach (var activity in UserActivities)
                {
                    var x = (DateTime.Now - activity.StartTime).TotalDays;
                    if (x < 365 && x > 0)
                    {
                        var month = activity.StartTime.Month - 1;
                        var prev = chartArray[month];
                        if (property == "Activities" || activity.ActivityType == property) chartArray[month] = prev + 1;
                    }
                }
                tempDictBeforeReturn[property] = chartArray;
            }
            this.LineChartData = tempDictBeforeReturn;
        }




    }
}
