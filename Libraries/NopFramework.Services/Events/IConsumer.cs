using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Events
{
    /// <summary>
    /// 消费者
    /// </summary>
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
