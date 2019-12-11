using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SmearAdmin.Helpers
{
    public class SMSUtility
    {
        public const string SMSApiKey = "90YLR6eX3EqLUnAX9aUXaA";
        public const string Birthday = "Birthday";
        public const string Festival = "Festival";
        public const string NationalFestival = "National Festival";
        public const string SendFrom = " - velikandovera.co.in.";

        public static async Task<string> SendSMS(string Message, string MobileNo)
        {
            string message = "fail";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.smsgatewayhub.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string postUrl = $"api/mt/SendSMS?APIKey={SMSApiKey}&senderid=VELIKN&channel=2&DCS=0&flashsms=0&number={MobileNo}&text={Message}{SendFrom}&route=1";

                using (HttpResponseMessage response = client.GetAsync(postUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> responseData = content.ReadAsStringAsync();
                            message = Convert.ToString(responseData.Result);
                        }
                    }
                }
            }
            return await Task.FromResult(message);
        }

        public static async Task<string> GetSMSBalance()
        {
            string message = "fail";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.smsgatewayhub.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string postUrl = "api/mt/GetBalance?APIKey=" + SMSApiKey;

                using (HttpResponseMessage response = client.GetAsync(postUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> responseData = content.ReadAsStringAsync();
                            message = Convert.ToString(responseData.Result);
                        }
                    }
                }
            }
            return await Task.FromResult(message);
        }

        public static string GetSMSMessageText(string name, string userType)
        {
            string msg;
            if (userType == "doctor")
                msg = $"Dear Dr. {name}, Wish you a HAPPY BIRTHDAY {SendFrom}";
            //msg = string.Format("Dear Dr. {0}, Wish you a HAPPY BIRTHDAY " + SendFrom, name);
            else
                msg = $"Dear {name}, Wish you a HAPPY BIRTHDAY {SendFrom}";
            //msg = string.Format("Dear {0}, Wish you a HAPPY BIRTHDAY " + SendFrom, name);

            return msg;
        }
    }
}
