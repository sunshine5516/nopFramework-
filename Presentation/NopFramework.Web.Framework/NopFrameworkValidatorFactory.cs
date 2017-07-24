using System.Reflection;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using NopFramework.Core.Infrastructure;

namespace NopFramework.Web.Framework
{
    public class NopFrameworkValidatorFactory: AttributedValidatorFactory
    {
        public override IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
                if ((attribute != null) && (attribute.ValidatorType != null))
                {
                    var instance = EngineContext.Current.ContainerManager.ResolveUnregistered(attribute.ValidatorType);
                    return instance as IValidator;
                }
            }
            return null;
        }
    }
}
