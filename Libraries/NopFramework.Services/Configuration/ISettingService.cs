using NopFramework.Core.Configuration;
using NopFramework.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Configuration
{
    public partial interface ISettingService
    {
        void InsertSetting(Setting setting, bool clearCache = true);
        void UpdateSetting(Setting setting, bool clearCache = true);
        Setting GetSettingById(int settingId);
        void DeleteSetting(Setting setting);
        void DeleteSetting<T>() where T : ISettings, new();
        void DeleteSettings(IList<Setting> settings);
        /// <summary>
        /// 根据key获取实体
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="loadSharedValueIfNotFound">如果没有找到特定于某个值的值，则该值指示是否应加载共享（所有存储）值</param>
        /// <returns>Setting</returns>
        Setting GetSetting(string key, bool loadSharedValueIfNotFound = false);
        /// <summary>
        /// 根据key获取实体
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="loadSharedValueIfNotFound">如果没有找到特定于某个值的值，则该值指示是否应加载共享（所有存储）值</param>
        /// <returns>Setting value</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T),
          bool loadSharedValueIfNotFound = false);
        /// <summary>
        /// 设置setting实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">是否在更新后清除缓存</param>
        void SetSetting<T>(string key, T value,bool clearCache = true);
        /// <summary>
        /// 获得所有setting实体
        /// </summary>
        /// <returns>Settings</returns>
        IList<Setting> GetAllSettings();
        /// <summary>
        /// 是否存在setting
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        bool SettingExists<T, TPropType>(T settings,Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();
        /// <summary>
        /// 加载
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        T LoadSetting<T>() where T : ISettings, new();
        void SaveSetting<T>(T settings) where T : ISettings, new();
        void SaveSetting<T, TPropType>(T settings, 
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();
    }
}
