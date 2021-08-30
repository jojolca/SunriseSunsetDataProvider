using Dapper;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Module
{
    public class Repository : IRepositoryOperater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionInfo"></param>
        public Repository(SqlConnectionStringBuilder connectionInfo)
        {
            ConnectionInformation = connectionInfo;
        }

        /// <summary>
        /// 連線資訊
        /// </summary>
        private SqlConnectionStringBuilder ConnectionInformation;

        public async Task<IEnumerable<SunRiseAndSunsetData>> Get(DateTime startTime, DateTime endTime)
        {
            IEnumerable<SunRiseAndSunsetData> resutlt = new SunRiseAndSunsetData[0];

            string cmd = $@"select * 
                            from [Test].[dbo].[SunRiseAndSunsetData] 
                            where [Date] between @StartTime and @EndTime";
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                sqlConnection.Open();
                resutlt =await sqlConnection.QueryAsync<SunRiseAndSunsetData>(cmd, new { StartTime = startTime, EndTime = endTime });
            }

            return resutlt;
        }

        public async Task<bool> Insert(IEnumerable<SunRiseAndSunsetData> newData, int retryTimes)
        {
            string cmd = $@"INSERT INTO [Test].[dbo].[SunRiseAndSunsetData]
                                   ([City]
                                   ,[Date]
                                   ,[JsonData])
                             VALUES
                                   (@City
                                   ,@Date
                                   ,@JsonData)";

            bool isSuccess = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionInformation.ConnectionString))
                {
                    sqlConnection.Open();
                    var executeResult = await sqlConnection.ExecuteAsync(cmd, newData.ToArray());
                    isSuccess = executeResult == newData.Count();
                }
            }
            catch(Exception ex)
            {
                if(retryTimes > 0)
                {
                    retryTimes--;
                    isSuccess = await Insert(newData, retryTimes);
                }
            }            

            return isSuccess;
        }
    }
}
