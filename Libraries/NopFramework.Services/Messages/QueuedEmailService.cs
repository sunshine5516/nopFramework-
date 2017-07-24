using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Messages;
using NopFramework.Core.Data;
using NopFramework.Services.Events;

namespace NopFramework.Services.Messages
{
    /// <summary>
    /// 邮件队列接口实现
    /// </summary>
    public partial class QueuedEmailService : IQueuedEmailService
    {
        #region 实例
        private readonly IRepository<QueuedEmail> _queuedEmailRepository;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region 构造函数
        public QueuedEmailService(IRepository<QueuedEmail> queuedEmailRepository,
            IEventPublisher _eventPublisher)
        {
            this._queuedEmailRepository = queuedEmailRepository;
            this._eventPublisher = _eventPublisher;
        }
        #endregion
        /// <summary>
        /// 插入电子邮件队列
        /// </summary>
        /// <param name="queuedEmail"></param>
        public virtual void InsertQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");
            _queuedEmailRepository.Insert(queuedEmail);
            _eventPublisher.EntityInserted(queuedEmail);
        }
        /// <summary>
        /// 更新电子邮件队列
        /// </summary>
        /// <param name="queuedEmail"></param>
        public virtual void UpdateQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");
            _queuedEmailRepository.Update(queuedEmail);
            _eventPublisher.EntityUpdated(queuedEmail);
        }

        /// <summary>
        /// 删除电子邮件队列
        /// </summary>
        /// <param name="queuedEmail"></param>
        public virtual void DeleteQueuedEmail(QueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");

            _queuedEmailRepository.Delete(queuedEmail);

            //event notification
            _eventPublisher.EntityDeleted(queuedEmail);
        }
        /// <summary>
        /// 删除电子邮件队列
        /// </summary>
        /// <param name="queuedEmails"></param>
        public virtual void DeleteQueuedEmails(IList<QueuedEmail> queuedEmails)
        {
            if (queuedEmails == null)
                throw new ArgumentNullException("queuedEmail");
            _queuedEmailRepository.Delete(queuedEmails);
            ///消息处理
            foreach (var queued in queuedEmails)
            {
                _eventPublisher.EntityDeleted(queued);
            }
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 根据ID获取电子邮件队列
        /// </summary>
        /// <param name="queuedEmailId"></param>
        /// <returns></returns>
        public virtual QueuedEmail GetQueuedEmailById(int queuedEmailId)
        {
            if(queuedEmailId==0)
                return null;
            return _queuedEmailRepository.GetById(queuedEmailId);
        }
        /// <summary>
        /// 根据ID获取电子邮件队列
        /// </summary>
        /// <param name="queuedEmailIds"></param>
        /// <returns></returns>
        public virtual IList<QueuedEmail> GetQueuedEmailsByIds(int[] queuedEmailIds)
        {
            if (queuedEmailIds == null || queuedEmailIds.Length == 0)
                return new List<QueuedEmail>();
            var query= from qe in _queuedEmailRepository.Table
                       where queuedEmailIds.Contains(qe.Id)
                       select qe;
            var queuedEmails = query.ToList();
            var sortedQueuedEmails = new List<QueuedEmail>();
            foreach (int id in queuedEmailIds)
            {
                var queuedEmail = queuedEmails.Find(x => x.Id == id);
                    if (queuedEmail != null)
                    sortedQueuedEmails.Add(queuedEmail);
            }
            return sortedQueuedEmails;
        }

        /// <summary>
        /// 获取所有的消息队列数据
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="loadNotSentItemsOnly"></param>
        /// <param name="loadOnlyItemsToBeSent"></param>
        /// <param name="maxSendTries"></param>
        /// <param name="loadNewest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<QueuedEmail> SearchEmails(string fromEmail, string toEmail, DateTime? createdFrom,
            DateTime? createdTo, bool loadNotSentItemsOnly, bool loadOnlyItemsToBeSent, int maxSendTries,
            bool loadNewest, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            fromEmail = (fromEmail ?? String.Empty).Trim();
            toEmail = (toEmail ?? String.Empty).Trim();
            var query = _queuedEmailRepository.Table;
            if (!String.IsNullOrEmpty(fromEmail))
                query = query.Where(qe => qe.From.Contains(fromEmail));
            if (!String.IsNullOrEmpty(toEmail))
                query = query.Where(qe => qe.To.Contains(toEmail));
            if (createdFrom.HasValue)
                query = query.Where(qe => qe.CreatedOn >= createdFrom);
            if (createdTo.HasValue)
                query = query.Where(qe => qe.CreatedOn <= createdTo);
            if (loadNotSentItemsOnly)
                query = query.Where(qe => !qe.SentOn.HasValue);
            if (loadOnlyItemsToBeSent)
            {
                var now = DateTime.Now;
                query = query.Where(qe => !qe.DontSendBeforeDateUtc.HasValue || qe.DontSendBeforeDateUtc.Value <= now);
            }
            query = query.Where(q => q.SentTries < maxSendTries);
            query = loadNewest ?
                //load the newest records
                query.OrderByDescending(qe => qe.CreatedOn) :
                //load by priority
                query.OrderByDescending(qe => qe.PriorityId).ThenBy(qe => qe.CreatedOn);
            var queuedEmails = new PagedList<QueuedEmail>(query, pageIndex, pageSize);
            return queuedEmails;
        }
        /// <summary>
        /// 删除电子邮件队列中所有数据
        /// </summary>
        public void DeleteAllEmails()
        {
            var queuedEmails = _queuedEmailRepository.Table.ToList();
            foreach (var qe in queuedEmails)
                _queuedEmailRepository.Delete(qe);
        }

        public IPagedList<QueuedEmail> GetAllEmails()
        {
            var query = _queuedEmailRepository.Table.OrderBy(ea => ea.Id); ;
            var queuedEmails = new PagedList<QueuedEmail>(query, 0, 12);
            return queuedEmails;
        }
    }
}
