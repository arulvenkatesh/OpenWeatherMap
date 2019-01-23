using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;

namespace OpenWeatherMap.Features
{
    class AutomationHelper
    {

        public async Task<string> openWeatherMap_API_Response(string destination, int days)
        {

            using (var client = new HttpClient())
            {
                string APPID = "0684e13c08718e34e5c19a1b9ea5195e";
                string units = "metric";
                string cnt = days.ToString();
                string destinationCity = destination;
                string apiDNS = "http://api.openweathermap.org";
                string apiURL = apiDNS + "/data/2.5/forecast/daily?q=" + destinationCity + "&APPID=" + APPID + "&units=" + units + "&cnt=" + cnt;


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiURL);


                if (response.IsSuccessStatusCode)
                {
                    var API_response = await response.Content.ReadAsStringAsync();
                    return API_response;

                }
                else
                {
                    return null;
                }
            }


        }

        public bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        public string getWeekDay(long unixTime)
        {
            return getDateAndTime(unixTime).ToString("dddd");

        }

        public string getDate(long unixTime)
        {
            return getDateAndTime(unixTime).ToString("d");

        }

        public DateTime getDateAndTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

    }
}
