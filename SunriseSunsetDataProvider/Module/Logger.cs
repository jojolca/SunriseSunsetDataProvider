using SunriseSunsetDataProvider.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Module
{
    public class Logger : ILog
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public Logger( string name)
        {

            rootName = @".\log";
            if (!Directory.Exists(rootName))
            {
                Directory.CreateDirectory(rootName);
            }
            writerOfELK = File.AppendText($@"{rootName}\ELKErr.txt");

            writeLogTimer = new Timer(new TimerCallback(this.timerWriteLog), null, 0, Timeout.Infinite);
        }

        #region 變數

        /// <summary>
        /// 寫log的timer
        /// </summary>
        private Timer writeLogTimer;

        /// <summary>
        /// 需要被寫入的log
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentQueue<string>> logsNeedToWrite = new ConcurrentDictionary<string, ConcurrentQueue<string>>();

        /// <summary>
        /// 間隔1秒
        /// </summary>
        private const int RECORD_INTERVAL = 1000;

        /// <summary>
        /// 最大批次寫入資料筆數(先設10000筆看看)
        /// </summary>
        private const int MAX_ROWS_OF_WRITE_TO_LOGFILE = 10000;

        /// <summary>
        /// 放Log的根資料夾
        /// </summary>
        private string rootName = @".\log";

        /// <summary>
        /// ELK發生錯誤時的寫檔物件
        /// </summary>
        private StreamWriter writerOfELK;

        #endregion

        /// <summary>
        /// 增加log
        /// </summary>
        /// <param name="content">log內容</param>
        ///  <param name="dirName">資料夾名稱</param>
        public void AddLog(string content, string dirName)
        {
            logsNeedToWrite.AddOrUpdate(dirName,
                newValue =>
                {
                    var log = new ConcurrentQueue<string>();
                    log.Enqueue(content);
                    return log;
                },
                (key, existingValue) =>
                {
                    existingValue.Enqueue(content);
                    return existingValue;
                });
        }

        /// <summary>
        /// 寫log
        /// </summary>
        /// <param name="content"></param>
        private void writeLog()
        {
            string path = string.Empty;

            foreach (KeyValuePair<string, ConcurrentQueue<string>> keyValuePair in logsNeedToWrite)
            {
                path = $"{rootName}\\{keyValuePair.Key}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                try
                {
                    using (FileStream fs = new FileStream($"{path}\\{DateTime.Now:yyyy-MM-dd}.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            for (int i = 0; i < MAX_ROWS_OF_WRITE_TO_LOGFILE; i++)
                            {
                                if (keyValuePair.Value.TryDequeue(out string content))
                                {
                                    sw.WriteLine(content);
                                }
                                else
                                {
                                    break;
                                }
                            }


                        }
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 寫log的timer工作內容
        /// </summary>
        /// <param name="item"></param>
        private void timerWriteLog(object item)
        {
            int logCount = logsNeedToWrite.Values.Sum(x => x.Count);
            if (logCount > 0)
            {
                try
                {
                    writeLog();
                }
                catch (Exception ex)
                {

                }

            }

            //完成後再觸發下一次Timer
            this.writeLogTimer.Change(RECORD_INTERVAL, Timeout.Infinite);
        }
    }
}
