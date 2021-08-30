using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.VariableClasses
{
    /// <summary>
    /// 資料庫連線資訊
    /// </summary>
    public class SqlConnectionInfo
    {
        /// <summary>
        /// 資料庫IP位址
        /// </summary>
        public string DataSource { get; set; }
       
        /// <summary>
        /// 來源應用程式名稱
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 資料庫名稱
        /// </summary>
        public string InitialCatalog { get; set; }

        public bool IntegratedSecurity { set; get; }

        public SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            var result = new SqlConnectionStringBuilder()
            {
                DataSource = DataSource,
                InitialCatalog = InitialCatalog,
                ApplicationName = ApplicationName,
                IntegratedSecurity = IntegratedSecurity
            };
            Console.WriteLine($"DB {result.InitialCatalog} : {result.DataSource}");
            return result;
        }
    }
}
