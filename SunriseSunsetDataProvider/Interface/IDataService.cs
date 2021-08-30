using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Interface
{
    public interface IDataService
    {
        Task<(bool isSuccess,string result)> GetData(int month, string authorizationKey);
    }
}
