using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sjette.Models
{
    // DashboardData model to pass multiple models in 1 combined one.
    // Also used to combine all razor code and variables.
    // Not the most efficient but cleanest way to code.
    public class ActivityData
    {
        // ObjectProperties
        public Users User { get; set; }
        public List<Activities> UserActivities { get; set; }

        // Table
        public Dictionary<string, List<string>> TableHeaders { get; set; }
        public Dictionary<string, List<Activities>> TableData { get; set; }


        // Constructor
        public ActivityData(Users user, List<Activities> activities)
        {
            this.User = user;
            this.UserActivities = activities;

            SetTableHeaders();
            SetTableData();
            //System.Diagnostics.Debugger.Break();
        }


        private void SetTableHeaders()
        {
            var tempDictBeforeReturn = new Dictionary<string, List<string>>();

            string[] array1 = { "Name", "Date", "Duration", "Distance", "Calories", "Gear" };
            var list1 = new List<string>(array1);

            string[] array2 = { "Name", "Type", "Date", "Duration", "Distance", "Calories", "Gear" };
            var list2 = new List<string>(array2);

            tempDictBeforeReturn["Normal"] = list1;
            tempDictBeforeReturn["All"] = list2; 

            this.TableHeaders = tempDictBeforeReturn;
        }


        //Dict<Activity - Dict<UserName - List<numbers>
        private void SetTableData()
        {
            var tempDictBeforeReturn = new Dictionary<string, List<Activities>>();
            var AllActivities = new List<Activities>();
            tempDictBeforeReturn["All"] = AllActivities;
            foreach (var activity in UserActivities)
            {
                List<Activities> lstActivities;
                bool lstActivityExist = tempDictBeforeReturn.ContainsKey(activity.ActivityType);
                if (!lstActivityExist) lstActivities = new List<Activities>();
                else lstActivities = tempDictBeforeReturn[activity.ActivityType];

                AllActivities = tempDictBeforeReturn["All"];

                lstActivities.Add(activity);
                AllActivities.Add(activity);

                tempDictBeforeReturn[activity.ActivityType] = lstActivities;
                tempDictBeforeReturn["All"] = AllActivities;
            }
            this.TableData = tempDictBeforeReturn;
        }
        



    }
}
