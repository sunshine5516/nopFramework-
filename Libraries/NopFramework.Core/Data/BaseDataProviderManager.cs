using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Data
{
   public abstract class BaseDataProviderManager
    {
        protected DataSettings Settings { get; private set; }
        protected BaseDataProviderManager(DataSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
               
            }
            this.Settings = settings;
        }
        public abstract IDataProvider LoadDataProvider();
    }
}
