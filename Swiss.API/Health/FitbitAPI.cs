//using Fitbit.Api;
//using Fitbit.Models;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;

//namespace Swiss.API
//{
//    public class Report
//    {
//        public int Steps { get; set; }
//        public int Calories { get; set; }
//        public int Heart { get; set; }
//        public int Distance { get; set; }
//        public int Activity { get; set; }

//        public string Sleep { get; set; }
//    }

//    public class FitbitAPI
//    {
//        private static AuthCredential credentials;

//        private static string ConsumerKey;
//        private static string ConsumerSecret;

//        private static UserProfile profile;
//        private static FitbitClient client;

//        private static DateTime today = DateTime.Now;

//        public FitbitAPI(string key, string secret)
//        {
//            ConsumerKey = key;
//            ConsumerSecret = secret;
//        }

//        public Report GetReport()
//        {
//            if (credentials == null)
//            {
//                credentials = Authenticate();
//                client = new FitbitClient(ConsumerKey, ConsumerSecret, credentials.AuthToken, credentials.AuthTokenSecret);
//                profile = client.GetUserProfile();
//            }

//            return new Report()
//            {
//                Steps = GetSteps(),
//                Calories = GetCalories(),
//                Heart = GetHeartRate(),
//                Distance = GetDistanceTraveled(),
//                Sleep = GetSleep(),
//                Activity = GetTimeInZone()
//            };
//        }

//        public int GetGoal_Calories()
//        {
//            return client.GetDayActivity(today).Goals.CaloriesOut;
//        }

//        public int GetGoal_Steps()
//        {
//            return client.GetDayActivity(today).Goals.Steps;
//        }

//        public int GetCalories()
//        {
//            return client.GetDayActivitySummary(today).CaloriesOut;
//        }

//        public int GetSteps()
//        {
//            return client.GetDayActivitySummary(today).Steps;
//        }

//        public string GetSleep()
//        {
//            int minutesAsleep = client.GetSleep(today).Summary.TotalMinutesAsleep;

//            int hours = minutesAsleep / 60;
//            int minutes = minutesAsleep % 60;

//            return hours + " hours and " + minutes + " minutes";
//        }

//        public int GetHeartRate()
//        {
//            return client.GetHeartRates(today).Average[0].HeartRate;
//        }

//        public int GetDistanceTraveled()
//        {
//            return (int)client.GetDayActivitySummary(today).Distances[0].Distance;
//        }

//        public int GetTimeInZone()
//        {
//            return (int)client.GetDayActivitySummary(today).VeryActiveMinutes;
//        }

//        private AuthCredential Authenticate()
//        {
//            var requestTokenUrl = "http://api.fitbit.com/oauth/request_token";
//            var accessTokenUrl = "http://api.fitbit.com/oauth/access_token";
//            var authorizeUrl = "http://www.fitbit.com/oauth/authorize";

//            var a = new Authenticator(ConsumerKey, ConsumerSecret, requestTokenUrl, accessTokenUrl, authorizeUrl);

//            RequestToken token = a.GetRequestToken();

//            var url = a.GenerateAuthUrlFromRequestToken(token, false);

//            Process.Start(url);

//            string pin = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Title", "Default", -1, -1);

//            var credentials = a.GetAuthCredentialFromPin(pin, token);
//            return credentials;
//        }
//    }
//}
