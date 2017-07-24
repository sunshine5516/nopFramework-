using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Events
{
    /// <summary>
    /// 事件发布接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 事件发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventMessage"></param>
        void Publish<T>(T eventMessage);
    }
}
