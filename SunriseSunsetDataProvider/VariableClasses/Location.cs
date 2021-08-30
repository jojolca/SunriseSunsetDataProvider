using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.VariableClasses
{
    public class Location
    {
        public string LocationName { set; get; }

        public TimeObject[] Time { set; get; }
    }
}
