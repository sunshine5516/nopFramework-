using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Configuration;
using NopFramework.Core.Domains;
using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Services.Events;
using NopFramework.Core.Caching;

namespace NopFramework.Services.Configuration
{ 
    public partial class SettingService : ISettingService
    {
        #region 常量
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string SETTINGS_ALL_KEY = "Nop.setting.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SETTINGS_PATTERN_KEY = "Nop.setting.";
        #endregion
        #region 实例
        private readonly IRepository<Setting> _settingRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion
        #region Nested classes

        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public int StoreId { get; set; }
        }

        #endregion
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="settingRepository">Setting repository</param>
        public SettingService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Setting> settingRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._settingRepository = settingRepository;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="clearCache"></param>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            _settingRepository.Insert(setting);
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            _eventPublisher.EntityInserted(setting);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="clearCache"></param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Update(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(setting);
        }
        public virtual void DeleteSetting(Setting setting)
        {
            if(setting==null)
                throw new ArgumentNullException("setting");
            _settingRepository.Delete(setting);
            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            //event notification
            _eventPublisher.EntityDeleted(setting);
        }

        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settingRepository.Delete(settings);
            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            //event notification
            foreach (var setting in settings)
            {
                _eventPublisher.EntityDeleted(setting);
            }
            
        }

        public virtual IList<Setting> GetAllSettings()
        {
            var query = _settingRepository.Table.OrderBy(p => p.Name).ToList();
            return query;
        }

        public Setting GetSetting(string key, bool loadSharedValueIfNotFound = false)
        {
            throw new NotImplementedException();
        }

        public Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetById(settingId);
        }

        public T GetSettingByKey<T>(string key, T defaultValue = default(T), bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                //load shared value?
                if (setting == null && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault(x => x.StoreId == 0);

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        public T LoadSetting<T>() where T : ISettings, new()
        {
            var settings = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //load by store
                var setting = GetSettingByKey<string>(key, loadSharedValueIfNotFound: true);
                if (setting == null)
                    continue;

                if (!CommonHelper.GetSunCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!CommonHelper.GetSunCustomTypeConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = CommonHelper.GetSunCustomTypeConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }
            return settings;
        }

        public void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            if(key==null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();
            string valueStr = CommonHelper.GetNopCustomTypeConverter(typeof(T)).ConvertToInvariantString(value);
            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                    
                };
                InsertSetting(setting, clearCache);
            }
                
        }

        public bool SettingExists<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // 获取可以读写的属性
                if (!prop.CanRead || !prop.CanWrite)
                    continue;
                if (!CommonHelper.GetNopCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;
                string key = typeof(T).Name + "." + prop.Name;
                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(key, value, false);
                else
                    SetSetting(key, "", false);
            }
            ClearCache();
        }

        public void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region 辅助方法

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            //cache
            string key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from s in _settingRepository.TableNoTracking
                            orderby s.Name
                            select s;
                var settings = query.ToList();
                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain store (storeId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }
        #endregion

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        public void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }
    }
}
