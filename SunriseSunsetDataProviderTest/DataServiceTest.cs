using Moq;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace SunriseSunsetDataProviderTest
{
    public class DataServiceTest
    {
        private Mock<IRepositoryOperater> repository = new Mock<IRepositoryOperater>();

        private Mock<IOpenDataOperater> openData = new Mock<IOpenDataOperater>();

        private IDataService DataService;

        [Fact]
        public async void InsertNewDataSuccess()
        {
            DataService = new DataService(repository.Object, openData.Object);
            repository.Setup(x => x.Get(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new SunRiseAndSunsetData[0]);
            repository.Setup(x => x.Insert(It.IsAny<IEnumerable<SunRiseAndSunsetData>>(), It.IsAny<int>())).ReturnsAsync(true);
            var newData = new SunRiseAndSunsetData[] { new SunRiseAndSunsetData() { City ="Test", Date=DateTime.Now, JsonData= "[{ \"ParameterName\":\"�����ƥ��l\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"��X�ɨ�\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"�L����\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"����\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"��S�ɨ�\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"���μǥ���\",\"ParameterValue\":\"17:40\"}]" } };
            openData.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(newData);

            var result =await DataService.GetData(12, "Test-Token");
            Assert.True(result.isSuccess);

        }

        [Fact]
        public async void InsertNewDataFailed()
        {
            DataService = new DataService(repository.Object, openData.Object);
            repository.Setup(x => x.Get(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new SunRiseAndSunsetData[0]);
            repository.Setup(x => x.Insert(It.IsAny<IEnumerable<SunRiseAndSunsetData>>(), It.IsAny<int>())).ReturnsAsync(false);
            var newData = new SunRiseAndSunsetData[] { new SunRiseAndSunsetData() { City = "Test", Date = DateTime.Now, JsonData = "[{ \"ParameterName\":\"�����ƥ��l\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"��X�ɨ�\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"�L����\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"����\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"��S�ɨ�\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"���μǥ���\",\"ParameterValue\":\"17:40\"}]" } };
            openData.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(newData);

            var result = await DataService.GetData(12, "Test-Token");

            Assert.False(result.isSuccess);
        }

        [Fact]
        public async void InsertInvalidNewDataFailed()
        {
            DataService = new DataService(repository.Object, openData.Object);
            repository.Setup(x => x.Get(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new SunRiseAndSunsetData[0]);
            repository.Setup(x => x.Insert(It.IsAny<IEnumerable<SunRiseAndSunsetData>>(), It.IsAny<int>())).ReturnsAsync(false);
            var newData = new SunRiseAndSunsetData[] { new SunRiseAndSunsetData() { City = "Test", Date = DateTime.Now, JsonData = "Test" } };
            openData.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(newData);

            var result = await DataService.GetData(12, "Test-Token");

            Assert.False(result.isSuccess);
        }

        [Fact]
        public async void InsertPartialNewDataSuccess()
        {
            var fackData = new SunRiseAndSunsetData() { City = "Test", Date = DateTime.Now.AddDays(1), JsonData = "[{ \"ParameterName\":\"�����ƥ��l\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"��X�ɨ�\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"�L����\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"����\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"��S�ɨ�\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"���μǥ���\",\"ParameterValue\":\"17:40\"}]" };
            DataService = new DataService(repository.Object, openData.Object);
            repository.Setup(x => x.Get(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new SunRiseAndSunsetData[] { fackData });
            repository.Setup(x => x.Insert(It.IsAny<IEnumerable<SunRiseAndSunsetData>>(), It.IsAny<int>())).ReturnsAsync(true);
            var newData = new SunRiseAndSunsetData[] { new SunRiseAndSunsetData() { City = "Test", Date = DateTime.Now, JsonData = "[{ \"ParameterName\":\"�����ƥ��l\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"��X�ɨ�\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"�L����\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"����\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"��S�ɨ�\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"���μǥ���\",\"ParameterValue\":\"17:40\"}]" } };
            openData.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(newData);

            var result = await DataService.GetData(12, "Test-Token");
            Assert.True(result.isSuccess);

        }

        [Fact]
        public async void GetExsistDataSuccess()
        {
            int month = 6;
            List<SunRiseAndSunsetData> fackDataList = new List<SunRiseAndSunsetData>();
            for (int i =0; i< 30; i++)
            {
                fackDataList.Add(new SunRiseAndSunsetData() { City = "Test", Date = new DateTime(2021, month, 1).AddDays(i), JsonData = "[{ \"ParameterName\":\"�����ƥ��l\",\"ParameterValue\":\"05:33\"},{ \"ParameterName\":\"��X�ɨ�\",\"ParameterValue\":\"05:57\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"103\"},{ \"ParameterName\":\"�L����\",\"ParameterValue\":\"11:37\"},{ \"ParameterName\":\"����\",\"ParameterValue\":\"53S\"},{ \"ParameterName\":\"��S�ɨ�\",\"ParameterValue\":\"17:17\"},{ \"ParameterName\":\"���\",\"ParameterValue\":\"257\"},{ \"ParameterName\":\"���μǥ���\",\"ParameterValue\":\"17:40\"}]" });
            }

            DataService = new DataService(repository.Object, openData.Object);
            repository.Setup(x => x.Get(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(fackDataList);
            repository.Setup(x => x.Insert(It.IsAny<IEnumerable<SunRiseAndSunsetData>>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await DataService.GetData(month, "Test-Token");
            Assert.True(result.isSuccess);
        }
    }
}
