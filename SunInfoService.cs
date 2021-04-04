
using System.Text.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace Slunce2 {

    class SunInfoService {

        public static async Task<SunInfo> GetCurrentSunInfo(string latitude, string longitude) {

            string requestUrl = CreateRequestUrl(latitude, longitude);
            string jsonString = "";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
                jsonString = await response.Content.ReadAsStringAsync();
            else
                return null;

            JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

            return new SunInfo {
                status = jsonDocument.RootElement.GetProperty("status").ToString(),
                sunrise = jsonDocument.RootElement.GetProperty("results").GetProperty("sunrise").ToString(),
                sunset = jsonDocument.RootElement.GetProperty("results").GetProperty("sunset").ToString(),
                solar_noon = jsonDocument.RootElement.GetProperty("results").GetProperty("solar_noon").ToString(),
                day_length = jsonDocument.RootElement.GetProperty("results").GetProperty("day_length").ToString(),
                civil_twilight_begin = jsonDocument.RootElement.GetProperty("results").GetProperty("civil_twilight_begin").ToString(),
                civil_twilight_end = jsonDocument.RootElement.GetProperty("results").GetProperty("civil_twilight_end").ToString(),
                nautical_twilight_begin = jsonDocument.RootElement.GetProperty("results").GetProperty("nautical_twilight_begin").ToString(),
                nautical_twilight_end = jsonDocument.RootElement.GetProperty("results").GetProperty("nautical_twilight_end").ToString(),
                astronomical_twilight_begin = jsonDocument.RootElement.GetProperty("results").GetProperty("astronomical_twilight_begin").ToString(),
                astronomical_twilight_end = jsonDocument.RootElement.GetProperty("results").GetProperty("astronomical_twilight_end").ToString(),
            };

        }
        private static string CreateRequestUrl(string latitude, string longitude) {
            return "https://api.sunrise-sunset.org/json?lat=" + latitude + "&lng=" + longitude;
            //return "https://api.sunrise-sunset.org/json?lat=36.7201600&lng=-4.4203400&date=2021-04-03";
        }
    }
}