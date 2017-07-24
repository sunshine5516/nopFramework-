using NopFramework.Core.Infrastructure;
using NopFramework.Core.Plugins;
using NopFramework.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subscriptionService"></param>
        public EventPublisher(ISubscriptionService subscriptionService)
        {
            this._subscriptionService = subscriptionService;
        }
        /// <summary>
        /// 发布事件，通知订阅者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="eventMessage"></param>
        public virtual void PublishToConsumer<T>(IConsumer<T> x,T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                var logger= EngineContext.Current.Resolve<ILogger>();
                try
                {
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 根据位于其程序集中的某个类型找到一个插件描述符
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");

            if (PluginManager.ReferencedPlugins == null)
                return null;
            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;

                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }

            return null;
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventMessage"></param>
        public virtual void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
            subscriptions.ToList().ForEach(x=>PublishToConsumer(x,eventMessage));
        }
    }
}
