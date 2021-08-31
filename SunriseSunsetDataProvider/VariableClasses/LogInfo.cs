using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.VariableClasses
{
    public class LogInfo
    {
        public string Action { set; get; }

        public object Info { set; get; }

        public string Message { set; get; }

        public bool Success { set; get; }

        public long Cost { set; get; }
    }
}
