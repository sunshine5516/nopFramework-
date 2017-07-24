using NopFramework.Core;
using NopFramework.Core.Domains.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Media
{
    /// <summary>
    /// 图片服务接口
    /// </summary>
    public partial interface IPictureService
    {
        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        byte[] LoadPictureBinary(Picture picture);
        /// <summary>
        /// 获取图片SEO友好的名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetPictureSeName(string name);
        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize"></param>
        /// <param name="defaultPictureType"></param>
        /// <returns></returns>
        string GetDefaultPictureUrl(int targetSize = 0, PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <param name="showDefaultPicture"></param>
        /// <param name="defaultPictureType"></param>
        /// <returns></returns>
        string GetPictureUrl(int pictureId, int targetSize = 0,
            bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity);
        string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity);
        /// <summary>
        /// 获取图片本地路径
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <param name="showDefaultPicture"></param>
        /// <returns></returns>
        string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        Picture GetPictureById(int pictureId);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture"></param>
        void DeletePicture(Picture picture);
        /// <summary>
        /// 获取图片集合
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 按ID符获取图片
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="recordsToReturn">要返回的记录数。 0返回所有</param>
        /// <returns></returns>
        IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0);
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">图片MIME类型</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">“img”HTML元素的“alt”属性</param>
        /// <param name="titleAttribute">“img”HTML元素的“title”属性</param>
        /// <param name="isNew">图片是否是新的值</param>
        /// <param name="validateBinary">指示是否验证提供的图片二进制的值</param>
        /// <returns>Picture</returns>
        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttrirbute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);
        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="pictureBinary"></param>
        /// <param name="mimeType"></param>
        /// <param name="seoFilename"></param>
        /// <param name="altAttrirbute"></param>
        /// <param name="titleAttribute"></param>
        /// <param name="isNew"></param>
        /// <param name="validateBinary"></param>
        /// <returns></returns>
        Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttrirbute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);
        /// <summary>
        /// 更新图片的SEO文件名
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="seoFileName"></param>
        /// <returns></returns>
        Picture SetSeoFilename(int pictureId, string seoFileName);
        /// <summary>
        /// 验证输入图片尺寸
        /// </summary>
        /// <param name="pictureBinary"></param>
        /// <param name="mineType"></param>
        /// <returns></returns>
        byte[] ValidatePicture(byte[] pictureBinary, string mineType);
        /// <summary>
        /// 获取或设置一个值，指示图像是否应存储在数据库中。
        /// </summary>
        bool StoreInDb { get; set; }
        /// <summary>
        /// 获取图片散列
        /// </summary>
        /// <param name="picturesIds"></param>
        /// <returns></returns>
        IDictionary<int, string> GetPicturesHash(int[] picturesIds);
    }
}
