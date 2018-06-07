using BizApplication.Core.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace BizApplication.Core.Common.DI
{
    public class ResolverConfig
    {
        #region プロパティ
        /// <summary>
        /// Abstract type
        /// </summary>
        public Type AbstractType
        {
            get
            {
                return abstractType;
            }
        }

        /// <summary>
        /// Concrete type
        /// </summary>
        public Type ConcreteType
        {
            get
            {
                return concreteType;
            }
        }

        public TypeInfo ConcreteTypeInfo
        {
            get
            {
                return concreteTypeInfo;
            }
        }

        /// <summary>
        /// Constructor to be injected
        /// </summary>
        public ConstructorInfo Constructor
        {
            get
            {
                return constructor;
            }
        }

        /// <summary>
        /// Property to be injected
        /// </summary>
        public PropertyInfo[] Properties
        {
            get
            {
                return properties;
            }
        }

        /// <summary>
        /// Fields to be injected
        /// </summary>
        public FieldInfo[] Fields
        {
            get
            {
                return fields;
            }
        }

        /// <summary>
        /// Methods to be injected
        /// </summary>
        public MethodInfo[] Methods
        {
            get
            {
                return methods;
            }
        }

        public Func<object> FactoryMethod { get; internal set; }
        #endregion

        #region フィールド
        private Type abstractType;
        private Type concreteType;
        private TypeInfo concreteTypeInfo;
        private ConstructorInfo constructor;
        private PropertyInfo[] properties;
        private FieldInfo[] fields;
        private MethodInfo[] methods;
        #endregion

        #region 初期化処理
        public ResolverConfig(Type abstractType, Type concreteType)
        {
            this.abstractType = abstractType;
            this.concreteType = concreteType;
            this.concreteTypeInfo = concreteType.GetTypeInfo();

            SetUsingConstructor(out constructor, concreteTypeInfo);

            SetTargetFields(out fields, concreteType);

            SetTargetProperty(out properties, concreteType);

            SetTargetMethod(out methods, concreteType);
        }


        #endregion

        #region Private処理
        /// <summary>
        /// Extract the constructor defined based on type information and set it to a "Resolver configuration".
        /// </summary>
        /// <param name="ctor">Constructor</param>
        /// <param name="targetTypeInfo">Type infomation</param>
        private void SetUsingConstructor(out ConstructorInfo ctor, TypeInfo targetTypeInfo)
        {
            var ctors = targetTypeInfo.DeclaredConstructors.OrderByDescending(c => c.GetParameters().Length);
            var injectCtors = ctors.Where(c => c.GetCustomAttribute<InjectAttribute>() != null);

            if (injectCtors.Count() == 1)
            {
                ctor = injectCtors.First();
            } else if (injectCtors.Count() == 0)
            {
                ctor = ctors.FirstOrDefault();
            } else
            {
                throw new ContainerException("Multiple constructors can not be targeted for injection in the same class.");
            }
        }

        /// <summary>
        /// Extract the fields defined based on the type information and set it to a "Resolver configuration".
        /// </summary>
        /// <param name="fields">Fields</param>
        /// <param name="targetType">Type infomation</param>
        private void SetTargetFields(out FieldInfo[] fields, Type targetType)
        {
            fields = targetType.GetRuntimeFields().Where(f => f.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }

        /// <summary>
        /// Extract the properties defined based on the type information and set it to a "Resolver configuration".
        /// </summary>
        /// <param name="properties">Properties</param>
        /// <param name="targetType">Type infomation</param>
        private void SetTargetProperty(out PropertyInfo[] properties, Type targetType)
        {
            properties = targetType.GetRuntimeProperties().Where(p => p.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }

        /// <summary>
        /// Extract the methods defined based on type information and set it to a "Resolver configuration".
        /// </summary>
        /// <param name="methods">Methods</param>
        /// <param name="targetType">Type infomation</param>
        private void SetTargetMethod(out MethodInfo[] methods, Type targetType)
        {
            methods = targetType.GetRuntimeMethods().Where(m => m.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }
        #endregion
    }
}
