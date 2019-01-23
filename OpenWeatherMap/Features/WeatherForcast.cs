using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CSharp;
using System.Globalization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace OpenWeatherMap.Features
{
    [Binding]
    public class WeatherForcast
    {
        dynamic forecastResponse;
        string city;
        string weekDay;
        int days = 16;
        string todatdate;
        string finalString;
        AutomationHelper ah = new AutomationHelper();

        [Given(@"I like to holiday in (.*)")]
        public void GivenIliketoholidayin(string location)
        {
            city = location;
        }

        [Given(@"I only like to holiday on (.*)")]
        public void Ionlyliketoholidayon(string holidayOn)
        {
            weekDay = holidayOn;
        }

        [When(@"I look up the weather forecast")]
        public void Ilookuptheweatherforecast()
        {
        }

        [Then(@"I receive the weather forecast")]
        public void Ireceivetheweatherforecast()
        {

            var apiResponse = ah.openWeatherMap_API_Response(city, days).Result;
            try
            {
                forecastResponse = JsonConvert.DeserializeObject(apiResponse);
                Assert.IsNotNull(apiResponse, "No response is recieved from the API");

            }
            catch (Exception ex)
            {
                Boolean parsingError = ex.Message.Contains("Unexpected character encountered while parsing value");
                Assert.IsTrue(parsingError, "Not a Valid Json " + ex.Message.ToString());
            }

        }

        [Then(@"the temperature is warmer than (.*) degrees on (.*)")]
        public void thetemperatureiswarmerthandegrees(float degrees, string expWeekDay)
        {
            Boolean tempFlag = false;
            Boolean flag = false;
            List<string> warmerdays = new List<string>();

            foreach (var offer in forecastResponse.list)
            {
                JValue day = offer.dt;
                string actDayValue = ah.getWeekDay(Convert.ToInt64(day)).ToString();
                if ((actDayValue).ToUpper().Equals((expWeekDay).ToUpper()))
                {
                    string temprature_str = offer.temp.min;
                    float temprature = Convert.ToSingle(temprature_str);

                    todatdate = ah.getDate(Convert.ToInt64(day)).ToString();
                    if (temprature > degrees)
                    {
                        flag = true;
                    }
                    else
                    {
                        tempFlag = true;
                        warmerdays.Add(todatdate);
                    }

                }
            }
            if ((flag == true) && (tempFlag == false))
            {
                Assert.IsTrue(true, "The Temperature is not warmer than " + degrees.ToString() + " degress on all " + expWeekDay + " for next " + days + "days");
            }
            else
            {
                finalString = string.Join(" | ", warmerdays.ToArray());
                Assert.Fail("The Temperature is warmer than " + degrees.ToString() + " degrees on " + finalString + " in next " + days + " days");
            }


        }

        [Then(@"the destination is (.*)")]
        public void thedestinationis(string expCity)
        {
            string ActualCityName = forecastResponse.city.name;
            Assert.AreEqual(expCity, ActualCityName, "Actual City is different to expected City");
        }

    }
}
