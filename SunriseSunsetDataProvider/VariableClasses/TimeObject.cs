using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.VariableClasses
{
    public class TimeObject
    {
        public DateTime DataTime { set; get; }

        public ParameterObject[] Parameter { set; get; }
    }
}
