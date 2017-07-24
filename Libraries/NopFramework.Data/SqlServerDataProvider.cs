using NopFramework.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using NopFramework.Core;
using System.Data.Entity;
using NopFramework.Data.Initializers;
using System.Data.Entity.Infrastructure;

namespace NopFramework.Data
{
    public class SqlServerDataProvider : IDataProvider
    {
        #region 私有方法
        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));

                return new string[0];
            }
            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }
            return statements.ToArray();
        }
        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }
        #endregion


        public bool BackupSupported
        {
            get { return true; }
        }

        public bool StoredProceduredSupported
        {
            get { return true; }
        }

        public DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        public void InitConnectionFactory()
        {
            var connectionFactory = new SqlConnectionFactory();
            Database.DefaultConnectionFactory = connectionFactory;
           
        }

        public void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }
        /// <summary>
        /// 设置数据库初始化程序
        /// </summary>
        public void SetDatabaseInitializer()
        {
            var tablesToValidate = new[] { "Customer", "Discount", "Order", "Product", "ShoppingCartItem" };
            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));
            var initlializer = new CreateTablesIfNotExist<FrameworkObjectContext>(tablesToValidate, customCommands.ToArray());
                                   
            Database.SetInitializer(initlializer);
        }

        public int SupportedLengthOfBinaryHash()
        {
            return 8000;
        }
    }
}
