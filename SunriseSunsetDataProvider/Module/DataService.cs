
using Newtonsoft.Json;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.VariableClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Model
{
    public class DataService : IDataService
    {
        private readonly IRepositoryOperater Repository;

        private readonly IOpenDataOperater OpenDataOperater;

        public DataService(IRepositoryOperater repository, IOpenDataOperater openDataOperater)
        {
            Repository = repository;
            OpenDataOperater = openDataOperater;
        }

        public async Task<(bool isSuccess, string result)> GetData(int month, string authorizationKey)
        {
            var firstDay = new DateTime(DateTime.Now.Year, month, 1);
            var lastDays = firstDay.AddMonths(1);
            var totalDays = lastDays.Subtract(firstDay).TotalDays;

            try
            {
                var data = await Repository.Get(firstDay, lastDays);
                if (data != null && data.Count() > 0)
                {
                    List<SunRiseAndSunsetData> newData = new List<SunRiseAndSunsetData>();
                    var lackSomeData = data.GroupBy(x => x.City).Where(info => info.Count() < totalDays).ToDictionary(info => info.Key, value => value.Max(info => info.Date));                    
                    if (lackSomeData.Count() > 0)//詢問剩餘的天數的資料
                    {
                        foreach (var kv in lackSomeData)
                        {
                            firstDay = kv.Value.AddDays(1);
                            var openData = await OpenDataOperater.GetData(firstDay.ToString("yyyy-MM-dd"), lastDays.ToString("yyyy-MM-dd"), authorizationKey, kv.Key);
                            newData.AddRange(openData);
                            data = data.Concat(openData);
                        }
                       
                        var result = TransToCSVFormat(data.OrderBy(x => x.City).ThenBy(x => x.Date));
                        if (await Repository.Insert(newData, 2))
                        {
                            return (true, result);
                        }
                        else
                        {
                            return (false, $"寫入{firstDay.ToString("yyyy-MM-dd")}~{lastDays.ToString("yyyy-MM-dd")}資料庫失敗!");
                        }
                        
                    }
                    else if(lackSomeData.Count() == 0)//資料已存在
                    {
                        var result = TransToCSVFormat(data);
                        return (true, result);
                    }
                    else
                    {
                        return (false, "資料庫資料有誤!");
                    }
                }
                else //完全沒有資料
                {
                    var newData = await OpenDataOperater.GetData(firstDay.ToString("yyyy-MM-dd"), lastDays.ToString("yyyy-MM-dd"), authorizationKey, string.Empty);
                    var result = TransToCSVFormat(newData);
                    if (await Repository.Insert(newData, 2))
                    {
                        return (true, result);
                    }
                    else
                    {
                        return (false, $"寫入{firstDay.ToString("yyyy-MM-dd")}~{lastDays.ToString("yyyy-MM-dd")}資料庫失敗!");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        private string TransToCSVFormat(IEnumerable<SunRiseAndSunsetData> data)
        {
            StringBuilder result = new StringBuilder();
            result.Append($"日期,城市名稱,民用曙光始,日出時刻,方位,過中天,仰角,日沒時刻,方位,民用暮光終{Environment.NewLine}");
            foreach (var info in data)
            {
                var deserializedData = JsonConvert.DeserializeObject<ParameterObject[]>(info.JsonData);
                var values = deserializedData.Select(x => x.ParameterValue).ToArray();
                if(values.Length > 0)
                {
                    result.Append($"{info.Date:yyyy-MM-dd},{info.City},{string.Join(',', values)}{Environment.NewLine}");
                }
            }

            return result.ToString();
        }
    }
}
