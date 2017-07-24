using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Infrastructure
{
    /// <summary>
    /// 应该由启动时运行的线程实现的接口
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// 运行线程
        /// </summary>
        void Execute();
        /// <summary>
        /// 顺序
        /// </summary>
        /// <returns></returns>
        int Order { get; }
    }
}
