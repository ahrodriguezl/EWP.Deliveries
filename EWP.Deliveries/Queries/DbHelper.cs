using System;

namespace EWP
{
    class DbHelper
    {
        private static IDbHelper _instance = null;

        public static IDbHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    string dbType = SboClient.Instance.Company.DbServerType.ToString();

                    if (dbType.Contains("MSSQL") && !dbType.Equals("dst_MSSQL"))
                    {
                        _instance = new DbHelper_SQL();
                    }
                    else if (dbType.Contains("HANA"))
                    {
                        _instance = new DbHelper_HANA();
                    }
                    else
                    {
                        throw new Exception("Database type not supported: " + dbType);
                    }
                }

                return _instance;
            }
        }
    }
}
