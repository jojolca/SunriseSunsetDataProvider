using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Interface
{
    public interface ILog
    {
        /// <summary>
        /// 加入本地端log
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dirName"></param>
        void AddLog(string content, string dirName);
    }
}
