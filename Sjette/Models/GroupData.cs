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
        public Dictionary<string, List<Users>> AllUsersOfGroupByGroupName { get; set; }
        public List<Activities> UserActivities { get; set; }
        public Dictionary<int, List<Activities>> ActivitiesOfAllGroups { get; set; }


        // GroupStats
        public List<Groups> GroupsCreated { get; set; }
        public List<MutualUsers> MostMutualGroups { get; set; }


        // LineCharts
        public Dictionary<string, Dictionary<string, List<int>>> LineChartData { get; set; }


        // Table
        public Dictionary<string, List<string>> TableHeaders {get; set;}                                         //Activity -  Headers
        public Dictionary<int, Dictionary<string, Dictionary<string, List<int>> >> TableData { get; set; }       //GroupId  -  Dict<Activity - Dict<Username - List<numbers>>>


        // Constructor
        public GroupData(Users user, List<Groups> groups, List<Activities> activities, 
            Dictionary<int, List<Activities>> groupActivities, 
            Dictionary<int, List<Users>>  allUsersOfGroup, List<MutualUsers> mutualUsers)
        {
            string[] array1 = { "Activities", "Hiking", "Cycling", "Running", "Calories", "Distance"};
            List<string> listOfLineChartProperties = new List<string>(array1);

            this.User = user;
            this.UserActivities = activities;
            this.UserGroups = groups;
            this.ActivitiesOfAllGroups = groupActivities;
            this.AllUsersOfGroup = allUsersOfGroup;

            setAllUsersOfGroupByGroupName();

            setGroupsCreated();
            setMostMutualGroups(mutualUsers);

            SetLineChartData(listOfLineChartProperties);

            SetTableHeaders();
            SetTableData();
            //System.Diagnostics.Debugger.Break();
        }


        private void setAllUsersOfGroupByGroupName()
        {
            var dictBeforeReturn = new Dictionary<string, List<Users>>();
            foreach (var item in AllUsersOfGroup)
            {
                int groupId = item.Key;
                var group = UserGroups.Find(x => x.pk_GroupID == groupId);
                var userList = item.Value;
                dictBeforeReturn[group.GroupName] = userList;
            }
            this.AllUsersOfGroupByGroupName = dictBeforeReturn;
        }


        private void setGroupsCreated()
        {
            var listBeforeReturn = new List<Groups>();
            foreach (var group in UserGroups) if (group.fk_CreatorID == User.pk_UserID) listBeforeReturn.Add(group);
            this.GroupsCreated = listBeforeReturn;
        }



        private void setMostMutualGroups(List<MutualUsers> mutualUsers)
        {
            var returnLst = new List<MutualUsers>();
            var dictBeforeReturn = new Dictionary<MutualUsers, int>();
            foreach (var user in mutualUsers)
            {
                if(user.pk_UserID != User.pk_UserID & UserGroups.Select(ug => ug.pk_GroupID).ToList().Contains(user.GroupID))
                {
                    bool done = false;
                    foreach (var obj in dictBeforeReturn.Keys) if (obj.pk_UserID == user.pk_UserID)
                        {
                            int prevValue = 0;
                            prevValue = dictBeforeReturn[obj];
                            dictBeforeReturn[obj] = prevValue + 1;
                            done = true;
                        }
                    if (!done) dictBeforeReturn[user] = 1;
                }
            }

            var dictBeforeReturnOrdered = dictBeforeReturn.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var maxValue = dictBeforeReturnOrdered.FirstOrDefault().Value;
            foreach (var item in dictBeforeReturnOrdered) if (item.Value >= maxValue && item.Value >= 1) returnLst.Add(item.Key);

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
            string[] array3 = { "Name", "Amount", "Longest Hike", "Total Distance", "Total Calories", "Hikes 10km+", "Hikes 25km+", "Hikes 100km+"};

            var tempDictBeforeReturn = new Dictionary<string, List<string>>();
            var list1 = new List<string>(array1);
            var list2 = new List<string>(array2);
            var list3 = new List<string>(array3);

            tempDictBeforeReturn["Cycling"] = list1;
            tempDictBeforeReturn["Running"] = list2;
            tempDictBeforeReturn["Hiking"] = list3;

            this.TableHeaders = tempDictBeforeReturn;
        }

        private Users getUserFromId(int id)
        {
            foreach(var group in AllUsersOfGroup)
            {
                var groupId = group.Key;
                foreach (var user in group.Value) if (id == user.pk_UserID) return user;
            }
            return null;
        }

        private int calculateTime(TimeSpan timeDone, decimal distanceDoneDecimal, double distanceNeeded)
        {
            double distanceDone = Decimal.ToDouble(distanceDoneDecimal);
            if (distanceDone < distanceNeeded) return 0;
            return (int)(((double)distanceNeeded / distanceDone) * (int) timeDone.TotalMinutes);
        }


        //GroupId  -  Dict<Activity - Dict<UserName - List<numbers>
        private void SetTableData()
        {
            var tempDictBeforeReturn = new Dictionary<int, Dictionary<string, Dictionary<string, List<int>>>>();

            foreach (var item in ActivitiesOfAllGroups)
            {
                var groupId = item.Key;
                var activities = item.Value;

                var dictActivities = new Dictionary<string, Dictionary<string, List<int>>>();



                foreach (var activity in activities)
                {
                    // Check and make userDict
                    Dictionary<string, List<int>> userDict;
                    bool dictionaryExist = dictActivities.ContainsKey(activity.ActivityType);
                    if (dictionaryExist) userDict = dictActivities[activity.ActivityType];
                    else userDict = new Dictionary<string, List<int>>();

                    //Cycling
                    if (activity.ActivityType == "Cycling")
                    {
                        var user = getUserFromId(activity.fk_UserID);
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            int time50km = calculateTime(activity.TTime, activity.TKm, 50.0);
                            int time100km = calculateTime(activity.TTime, activity.TKm, 100.0);
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.TKm;
                            prev[3] += activity.TotalCalories;
                            if (prev[4] < time50km) prev[4] = time50km;
                            if (prev[5] < time100km) prev[5] = time100km;
                        }
                        else
                        {
                            prev = new List<int>();
                            int time50km = calculateTime(activity.TTime, activity.TKm, 50.0);
                            int time100km = calculateTime(activity.TTime, activity.TKm, 100.0);
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.TKm);
                            prev.Add(activity.TotalCalories);
                            prev.Add(time50km);
                            prev.Add(time100km);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }
                    // Running
                    else if (activity.ActivityType == "Running")
                    {
                        var user = getUserFromId(activity.fk_UserID);
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            int time5km = calculateTime(activity.TTime, activity.TKm, 5.0);
                            int time10km = calculateTime(activity.TTime, activity.TKm, 10.0);
                            int time21_1km = calculateTime(activity.TTime, activity.TKm, 21.1);
                            int time42_2km = calculateTime(activity.TTime, activity.TKm, 42.2);
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.TKm;
                            prev[3] += activity.TotalCalories;
                            if (prev[4] < time5km) prev[4] = time5km;
                            if (prev[5] < time10km) prev[5] = time10km;
                            if (prev[6] < time21_1km) prev[6] = time21_1km;
                            if (prev[7] < time42_2km) prev[7] = time42_2km;
                        }
                        else
                        {
                            prev = new List<int>();
                            int time5km = calculateTime(activity.TTime, activity.TKm, 5.0);
                            int time10km = calculateTime(activity.TTime, activity.TKm, 10.0);
                            int time21_1km = calculateTime(activity.TTime, activity.TKm, 21.1);
                            int time42_2km = calculateTime(activity.TTime, activity.TKm, 42.2);
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.TKm);
                            prev.Add(activity.TotalCalories);
                            prev.Add(time5km);
                            prev.Add(time10km);
                            prev.Add(time21_1km);
                            prev.Add(time42_2km);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }
                    else if (activity.ActivityType == "Hiking")
                    {
                        var user = getUserFromId(activity.fk_UserID);
                        var keyUsername = $"{user.FirstName} {user.LastName.Substring(0, 1)}.";
                        List<int> prev;
                        bool keyExist = userDict.TryGetValue(keyUsername, out prev);
                        if (keyExist)
                        {
                            prev[0] += 1;
                            if (prev[1] < activity.TTime.TotalMinutes) prev[1] = (int)activity.TTime.TotalMinutes;
                            prev[2] += (int)activity.TKm;
                            prev[3] += activity.TotalCalories;
                            if ((int)activity.TKm >= 10) prev[4]++;
                            if ((int)activity.TKm >= 25) prev[5]++;
                            if ((int)activity.TKm >= 100) prev[6]++;
                        }
                        else
                        {
                            prev = new List<int>();
                            prev.Add(1);
                            prev.Add((int)activity.TTime.TotalMinutes);
                            prev.Add((int)activity.TKm);
                            prev.Add(activity.TotalCalories);
                            if ((int)activity.TKm >= 10)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.TKm >= 25)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                            if ((int)activity.TKm >= 100)
                            {
                                prev.Add(1);
                            }
                            else prev.Add(0);
                        }
                        userDict[keyUsername] = prev;
                        dictActivities[activity.ActivityType] = userDict;
                    }
                    tempDictBeforeReturn[groupId] = dictActivities;
                }
                this.TableData = tempDictBeforeReturn;
            }
        }




    }
}
