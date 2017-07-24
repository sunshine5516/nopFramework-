using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Events
{
    /// <summary>
    /// 事件订阅服务
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<IConsumer<T>>();
        }
    }
}
