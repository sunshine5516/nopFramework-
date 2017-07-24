using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data
{
    public class EfStartUpTask : IStartupTask
    {
        public void Execute()
        {
            var settings = EngineContext.Current.Resolve<DataSettings>();
            if (settings != null && settings.IsValid())
            {
                var provider = EngineContext.Current.Resolve<IDataProvider>();
                if (provider == null)
                {
                    throw new SunFrameworkException("未找到IDataProvider");
                }
                provider.SetDatabaseInitializer();
            }
        }

        public int Order
        {
            get { return -1000; }
        }
    }
}
