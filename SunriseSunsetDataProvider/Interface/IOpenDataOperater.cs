using SunriseSunsetDataProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Interface
{
    public interface IOpenDataOperater
    {
        Task<IEnumerable<SunRiseAndSunsetData>> GetData(string startTime, string endTime, string authorizationToken, string locationName);
    }
}
