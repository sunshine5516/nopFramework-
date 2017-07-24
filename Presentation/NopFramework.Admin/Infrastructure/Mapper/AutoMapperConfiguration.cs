using AutoMapper;
using NopFramework.Admin.Models.Messages;
using NopFramework.Admin.Models.Plugins;
using NopFramework.Core.Domains.Messages;
using NopFramework.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// 映射器配置
    /// </summary>
    public static class AutoMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        public static void Init()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                //Mapper.CreateMap的第一个参数是源类型，第二参数是目标类型。
                cfg.CreateMap<EmailAccount, EmailAccountModel>()
                    .ForMember(dest => dest.Password, mo => mo.Ignore())
                    .ForMember(dest => dest.IsDefaultEmailAccount, mo => mo.Ignore())
                    .ForMember(dest => dest.SendTestEmailTo, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<EmailAccountModel, EmailAccount>()
                    .ForMember(dest => dest.Password, mo => mo.Ignore());
                //queued email
                cfg.CreateMap<QueuedEmail, QueuedEmailModel>()
                .ForMember(dest => dest.EmailAccountName,
                        mo => mo.MapFrom(src => src.EmailAccount != null ? src.EmailAccount.FriendlyName : string.Empty))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.PriorityName, mo => mo.Ignore())
                    .ForMember(dest => dest.DontSendBeforeDate, mo => mo.Ignore())
                    .ForMember(dest => dest.SendImmediately, mo => mo.Ignore())
                    .ForMember(dest => dest.SentOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<QueuedEmailModel, QueuedEmail>()
                    .ForMember(dest => dest.Priority, dt => dt.Ignore())
                    .ForMember(dest => dest.PriorityId, dt => dt.Ignore())
                    .ForMember(dest => dest.CreatedOn, dt => dt.Ignore())
                    .ForMember(dest => dest.DontSendBeforeDateUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.SentOn, mo => mo.Ignore())
                    .ForMember(dest => dest.EmailAccount, mo => mo.Ignore())
                    .ForMember(dest => dest.EmailAccountId, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachmentFilePath, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachmentFileName, mo => mo.Ignore())
                    .ForMember(dest => dest.AttachedDownloadId, mo => mo.Ignore());
                //plugins
                cfg.CreateMap<PluginDescriptor, PluginModel>()
                    .ForMember(dest => dest.ConfigurationUrl, mo => mo.Ignore())
                    .ForMember(dest => dest.CanChangeEnabled, mo => mo.Ignore())
                    .ForMember(dest => dest.IsEnabled, mo => mo.Ignore())
                    .ForMember(dest => dest.LogoUrl, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}