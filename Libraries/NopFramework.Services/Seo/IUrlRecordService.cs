using NopFramework.Core;
using NopFramework.Core.Domains.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Seo
{
    /// <summary>
    /// 提供有关URL记录的信息
    /// </summary>
    public partial interface IUrlRecordService
    {
        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="urlRecord"></param>
        void DeleteUrlRecord(UrlRecord urlRecord);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="urlRecords"></param>
        void DeleteUrlRecords(IList<UrlRecord> urlRecords);
        /// <summary>
        /// 获取URL记录
        /// </summary>
        /// <param name="urlRecordId"></param>
        /// <returns></returns>
        UrlRecord GetUrlRecordById(int urlRecordId);
        /// <summary>
        /// 获取URL记录
        /// </summary>
        /// <param name="urlRecordIds">urlRecordIds</param>
        /// <returns>URL record</returns>
        IList<UrlRecord> GetUrlRecordsByIds(int[] urlRecordIds);
        /// <summary>
        /// 插入一个URL记录
        /// </summary>
        /// <param name="urlRecord"></param>
        void InsertUrlRecord(UrlRecord urlRecord);
        /// <summary>
        /// 更新一个URL记录
        /// </summary>
        /// <param name="urlRecord"></param>
        void UpdateUrlRecord(UrlRecord urlRecord);
        /// <summary>
        /// 查找URL记录
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <returns>Found URL record</returns>
        UrlRecord GetBySlug(string slug);
        /// <summary>
        /// 查找URL记录（缓存版本）。
        /// 此方法的工作方式与“GetBySlug”一样，但缓存结果。
        /// 因此，它仅用于框架中的性能优化
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <returns>Found URL record</returns>
        UrlRecordService.UrlRecordForCaching GetBySlugCached(string slug);
        IPagedList<UrlRecord> GetAllUrlRecords(string slug = "", int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 查找slug
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="languageId"></param>
        /// <returns>Found slug</returns>
        string GetActiveSlug(int entityId, string entityName, int languageId);

        /// <summary>
        /// 保存slug
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="slug"></param>
        /// <param name="languageId"></param>
        void SaveSlug<T>(T entity, string slug, int languageId) where T : BaseEntity, ISlugSupported;
    }
}
