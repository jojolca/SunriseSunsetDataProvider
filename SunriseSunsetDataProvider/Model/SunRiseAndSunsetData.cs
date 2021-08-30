using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Model
{
    public class SunRiseAndSunsetData
    {
        public string City { set; get; }

        public DateTime Date { set; get; }

        public string JsonData { set; get; }
    }
}
