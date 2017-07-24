using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Messages;
using NopFramework.Core.Data;
using NopFramework.Services.Events;
using NopFramework.Core;

namespace NopFramework.Services.Messages
{
    public partial class EmailAccountService : IEmailAccountService
    {
        #region 实例
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region 构造函数
        public EmailAccountService(IRepository<EmailAccount> _emailAccountRepository,
            IEventPublisher _eventPublisher)
        {
            this._emailAccountRepository = _emailAccountRepository;
            this._eventPublisher = _eventPublisher;
        }
        #endregion
        #region 方法
        public void InsertEmailAccount(EmailAccount emailAccount)
        {
            if(emailAccount==null)
                throw new ArgumentNullException("emailAccount");
            emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email).Trim();
            emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName).Trim();
            emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host).Trim();
            emailAccount.UserName = CommonHelper.EnsureNotNull(emailAccount.UserName);
            emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password).Trim();


            emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
            emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
            emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
            emailAccount.UserName = CommonHelper.EnsureMaximumLength(emailAccount.UserName, 255);
            emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);
            _emailAccountRepository.Insert(emailAccount);
            _eventPublisher.EntityInserted(emailAccount);
        }
        /// <summary>
        /// 更新电子邮件账号
        /// 
        /// </summary>
        /// <param name="emailAccount"></param>
        public void UpdateEmailAccount(EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");
            emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email).Trim();
            emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName).Trim();
            emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host).Trim();
            emailAccount.UserName = CommonHelper.EnsureNotNull(emailAccount.UserName);
            emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password).Trim();


            emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
            emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
            emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
            emailAccount.UserName = CommonHelper.EnsureMaximumLength(emailAccount.UserName, 255);
            emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);
            _emailAccountRepository.Update(emailAccount);
            _eventPublisher.EntityUpdated(emailAccount);
        }
        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="emailAccount"></param>
        public void DeleteEmailAccount(EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");
            if (GetAllEmailAccounts().Count == 1)
                throw new SunFrameworkException("不能删除此电子邮件帐户。 至少需要一个帐户。");
            _emailAccountRepository.Delete(emailAccount);
            _eventPublisher.EntityDeleted(emailAccount);
        }
        /// <summary>
        /// 获取所有账号
        /// </summary>
        /// <returns></returns>
        public IList<EmailAccount> GetAllEmailAccounts()
        {
            var query = from ea in _emailAccountRepository.Table
                        orderby ea.Id
                        select ea;
            var emailAccount = query.ToList();
            return emailAccount;
        }
        public IPagedList<EmailAccount> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _emailAccountRepository.Table.OrderBy(ea => ea.Id);
            var emailAccount = new PagedList<EmailAccount>(query, pageIndex, pageSize);
            return emailAccount;
        }
        #endregion


        public void DeleteEmailAccounts(IList<EmailAccount> emailAccounts)
        {
            if (emailAccounts == null)
            {
                throw new ArgumentNullException("emailAccounts");
            }
            _emailAccountRepository.Delete(emailAccounts);
            foreach (var account in emailAccounts)
            {
                _eventPublisher.EntityDeleted(account);
            }
        }

        

        public virtual EmailAccount GetEmailAccountById(int emailAccountId)
        {
            if (emailAccountId == 0)
                return null;

            return _emailAccountRepository.GetById(emailAccountId);
        }

        public IList<EmailAccount> GetEmailAccountsByIds(int[] emailAccountIds)
        {
            if (emailAccountIds == null || emailAccountIds.Length == 0)
                return new List<EmailAccount>();
            var query = from ea in _emailAccountRepository.Table
                        where emailAccountIds.Contains(ea.Id)
                        select ea;
            var emails = query.ToList();
            var sortEmails = new List<EmailAccount>();
            foreach (int id in emailAccountIds)
            {
                var emailAccount = emails.Find(x => x.Id == id);
                if (emailAccount != null)
                {
                    sortEmails.Add(emailAccount);
                }
            }
            return sortEmails;
        }

        
    }
}
