using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Infrastructure
{
    public class EngineContext
    {
        /// <summary>
        /// 初始化时会创建一个新的NopEngine，
        /// </summary>
        /// <param name="forceRecreate">是否创建新的工厂实例</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null|| forceRecreate)
            {
                Singleton<IEngine>.Instance = new NopEngine();
                var config = ConfigurationManager.GetSection("NopConfig") as NopConfig;
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }
        /// <summary>
        /// 替换单例中的实例对象
        /// </summary>
        /// <param name="engine"></param>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
