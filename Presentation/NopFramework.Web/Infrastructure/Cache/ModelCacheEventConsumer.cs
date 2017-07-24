using NopFramework.Core.Domains.Seo;
using NopFramework.Core.Events;
using NopFramework.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Infrastructure.Cache
{
    public partial class ModelCacheEventConsumer : IConsumer<EntityUpdated<UrlRecord>>
    {
        public void HandleEvent(EntityUpdated<UrlRecord> eventMessage)
        {
            throw new NotImplementedException();
        }
        #region KEY
        /// <summary>
        /// Key for widget info
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : widget zone
        /// {2} : current theme name
        /// </remarks>
        public const string WIDGET_MODEL_KEY = "NopFramework.pres.widget-{0}";
        public const string WIDGET_PATTERN_KEY = "NopFramework.pres.widget";
        #endregion
    }
}