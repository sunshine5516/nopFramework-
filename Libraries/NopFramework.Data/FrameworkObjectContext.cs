using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using System.Reflection;

namespace NopFramework.Data
{
    /// <summary>
    /// Object context
    /// 这相类继承于DbContext, IDbContext通过重写OnModelCreating方法通过反射获取AppDomain下面所有的程序集，
    /// 然后动态创建EntityTypeConfiguration的实例，通过EF提供的API把我们配置的所有实体Map映射加入到modelBuilder的配置中去。
    /// </summary>
    public class FrameworkObjectContext : DbContext, IDbContext
    {
        #region 构造函数
        public FrameworkObjectContext() : base("SqlConnectionString")
        {
        }
        public FrameworkObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion
        /// <summary>
        ///  Fluent API制定表、列和关系
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
               type.BaseType.GetGenericTypeDefinition() == typeof(SunEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                ///动态创建type的实例，并把配置的实体Map映射加入到modelBuilder的配置中去。
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);

            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
            //    type.BaseType.GetGenericTypeDefinition() == typeof(SunEntityTypeConfiguration<>));
            //foreach (var type in typesToRegister)
            //{
            //    动态创建EntityTypeConfiguration的实例，并把我们配置的实体Map映射加入到modelBuilder的配置中去。
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.Configurations.Add(configurationInstance);
            //}
            //...or do it manually below.For example,
            //modelBuilder.Configurations.Add(new LanguageMap());



            //base.OnModelCreating(modelBuilder);
        }
        public bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        public bool ProxyCreationEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }


        public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
