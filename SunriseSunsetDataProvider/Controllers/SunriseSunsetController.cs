using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.Model;

namespace SunriseSunsetDataProvider.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SunriseSunsetController : ControllerBase
    {
        private readonly DataService DataServie;

        public SunriseSunsetController(DataService dataServie)
        {
            DataServie = dataServie;
        }

        /// <summary>
        /// 取得今年全台某月份日出日落資料
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="authorizationKey">氣象開放資料平台會員授權碼</param>
        /// <returns>日出日落資料</returns>
        [HttpGet("/SunriseSunsetData/{month}/{authorizationKey}")]
        public async Task<ActionResult<string>> GetData(int month, string authorizationKey)
        {
            if(month > 12 || month < 1 || string.IsNullOrWhiteSpace(authorizationKey) || string.IsNullOrEmpty(authorizationKey))
            {
                return BadRequest(new { message = "參數錯誤!" });
            }

            var response = await DataServie.GetData(month, authorizationKey);
            if(response.isSuccess)
            {
                return Ok(response.result);
            }

            return BadRequest(new { message = response.result });
        }
    }
}
