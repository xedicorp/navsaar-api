using Google.Apis.Auth.OAuth2;
using navsaar.api.Models;
using Newtonsoft.Json; 
using System.Net.Http.Headers;
using System.Text;

namespace navsaar.api.Services
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        private string _accessToken;
        private DateTime _tokenExpiry;

        public FirebaseNotificationService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiry > DateTime.UtcNow)
                return _accessToken;

            var serviceAccountPath = "service-account.json";// _config["Firebase:ServiceAccountPath"];

            var credential = GoogleCredential
                .FromFile(serviceAccountPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            _accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            _tokenExpiry = DateTime.UtcNow.AddMinutes(50);

            return _accessToken;
        }

        //public async Task<string> SendAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null)
        //{
        //    string projectId = "navsaar-b1e91";

        //    var message = new FirebaseMessage
        //    {
        //        message = new Message
        //        {
        //            token = deviceToken,
        //            notification = new FCMNotification
        //            {
        //                title = title,
        //                body = body
        //            },
        //            data = data
        //        }
        //    };

        //    var json = JsonConvert.SerializeObject(message);

        //    var request = new HttpRequestMessage(
        //        HttpMethod.Post,
        //        $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send"
        //    );

        //    request.Headers.Authorization =
        //        new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

        //    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.SendAsync(request);

        //    return await response.Content.ReadAsStringAsync();
        //}
        public async void Send(  string fcmToken, string otp )
        {
            string title = "Navsaar Login OTP";
            string projectId = "navsaar-b1e91";
            string deviceToken = fcmToken;
            string serviceAccountPath = "service-account.json";

            var credential = GoogleCredential
                .FromFile(serviceAccountPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            var accessToken = await credential.UnderlyingCredential
                .GetAccessTokenForRequestAsync();

            var msg = new
            {
                message = new
                {
                    token = deviceToken,
                    notification = new
                    {
                        title = title,
                        body = "OTP to login in Navsaar App is: " + otp
                    }
                }
            };

            var json = JsonConvert.SerializeObject(msg);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync(
                $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var result = await response.Content.ReadAsStringAsync();
             

        }
    }
}
