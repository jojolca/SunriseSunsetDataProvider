using Dapper;
using SunriseSunsetDataProvider.Model;
using SunriseSunsetDataProvider.Module;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;

namespace SunriseSunsetDataProviderTest
{
    public class RepositoryTest
    {
        [Fact]
        public async void InsertAndGet()
        {
            SqlConnectionStringBuilder sqlConnection = new SqlConnectionStringBuilder()
            {
                DataSource = "localhost",
                ApplicationName = "SunriseSunsetDataProvider",
                InitialCatalog = "Test",
                IntegratedSecurity = true
            };
           
            List<SunRiseAndSunsetData> fackDataList = new List<SunRiseAndSunsetData>();
            DateTime[] insertedDate = new DateTime[] { new DateTime(2021, 1, 1), new DateTime(2021, 1, 2) };
            string cityName = "Test";
            for (int i = 0; i < 2; i++)
            {
                var faleData = new SunRiseAndSunsetData() { City = cityName, Date = insertedDate[i], JsonData = "[{ \"ParameterName\":\"民用曙光始\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"日出時刻\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"方位\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"過中天\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"仰角\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"日沒時刻\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"方位\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"民用暮光終\",\"ParameterValue\":\"17:40\"}]" };
                fackDataList.Add(faleData);
            }

            Repository repository = new Repository(sqlConnection);
            bool isInsertSuccess = await repository.Insert(fackDataList, 1);
            var data = await repository.Get(insertedDate[0], insertedDate[1]);
            bool isInsertDataCorrect = data.Where(x => x.City == cityName).Count() == fackDataList.Count();

            using (SqlConnection sql = new SqlConnection(sqlConnection.ConnectionString))
            {
                sql.Open();
                var executeResult = await sql.ExecuteAsync("Delete from [Test].[dbo].[SunRiseAndSunsetData] where City =@City", new { City = cityName });
            }

            Assert.True(isInsertSuccess);
            Assert.True(isInsertDataCorrect);
        }
    }
}
