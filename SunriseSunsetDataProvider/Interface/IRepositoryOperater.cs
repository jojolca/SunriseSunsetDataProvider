using SunriseSunsetDataProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Interface
{
    public interface IRepositoryOperater
    {
        Task<IEnumerable<SunRiseAndSunsetData>> Get(DateTime startTime, DateTime endTime);

        Task<bool> Insert(IEnumerable<SunRiseAndSunsetData> newData, int retryTimes);
    }
}
