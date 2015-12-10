using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DBUntity
{
    public static class DBHelper
    {
        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="name">数据库实例名(默认name为空,调用默认数据库实例)</param>
        /// <returns>数据库对象</returns>
        private static Database CreateDataBase(string name = "")
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>(name);
        }
        private static Database database;
        public static Database Database
        {
            get
            {
                if (database == null)
                    database = CreateDataBase();
                return database;
            }
        }
        public static void UseTran(Action<DbTransaction> action)
        {
            using (DbConnection connection = Database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    action(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
