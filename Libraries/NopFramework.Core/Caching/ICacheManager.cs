using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Caching
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 通过Key获取缓存的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void Set(string key, object data, int cacheTime);
        /// <summary>
        /// 根据key判断缓存时候存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(string key);
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        void RemoveByPattern(string pattern);
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        void Clear();
    }
}
