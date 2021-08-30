using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.VariableClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Model
{
    public class OpenDataService : IOpenDataOperater
    {
        private readonly HttpClient Client;

        private readonly string CWDOpenDataURL;

        public OpenDataService(string cwdOpenDataURL, HttpClient client)
        {
            CWDOpenDataURL = cwdOpenDataURL;
            Client = client;
        }

        public async Task<IEnumerable<SunRiseAndSunsetData>> GetData(string startTime, string endTime, string authorizationToken, string locationName)
        {
            string url = $"{CWDOpenDataURL}/A-B0062-001?Authorization={authorizationToken}&format=JSON&timeFrom={startTime}&timeTo={endTime}";
            if (!string.IsNullOrEmpty(locationName))
            {
                url = $"{url}&locationName={locationName}";
            }

            var response = await Client.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var rawData = JsonConvert.DeserializeObject<JObject>(responseStr);

                var records = rawData["records"];
                if (records == null )
                {
                    return null;
                }

                var rawLocations = records["locations"];
                if (rawLocations == null)
                {
                    return null;
                }

                var locationArray = rawLocations["location"];
                if (locationArray == null)
                {
                    return null;
                }

                return TransRawDataToSunRiseAndSunsetData(locationArray.ToString());
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<SunRiseAndSunsetData> TransRawDataToSunRiseAndSunsetData(string locationRawData)
        {
            if(string.IsNullOrEmpty(locationRawData))
            {
                return null;
            }

            var deserializedData = JsonConvert.DeserializeObject<Location[]>(locationRawData);
            var result = deserializedData.SelectMany(content => content.Time, (content, entry) => new SunRiseAndSunsetData
            {
                City = content.LocationName,
                Date = entry.DataTime,
                JsonData = JsonConvert.SerializeObject(entry.Parameter)
            });

            return result;
        }
    }
}

