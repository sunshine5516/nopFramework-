using NopFramework.Core;
using NopFramework.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data
{
    /// <summary>
    /// 可以根据配置返回接口IDataProvider的不同实现的具体类
    /// </summary>
    public partial class EfDataProviderManager : BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings) : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {
            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new SunFrameworkException("providerName为空！");
            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                case "sqlce":
                    //return new SqlCeDataProvider();
                default:
                    throw new SunFrameworkException(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }
    }
}
