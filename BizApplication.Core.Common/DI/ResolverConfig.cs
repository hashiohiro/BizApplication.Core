using System;
using System.Linq;
using System.Reflection;
using BizApplication.Core.Common.Error;


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
                return constructor.Value;
            }
        }

        /// <summary>
        /// Property to be injected
        /// </summary>
        public PropertyInfo[] Properties
        {
            get
            {
                return properties.Value;
            }
        }

        /// <summary>
        /// Fields to be injected
        /// </summary>
        public FieldInfo[] Fields
        {
            get
            {
                return fields.Value;
            }
        }

        /// <summary>
        /// Methods to be injected
        /// </summary>
        public MethodInfo[] Methods
        {
            get
            {
                return methods.Value;
            }
        }

        public Func<object> FactoryMethod { get; internal set; }
        #endregion

        #region フィールド
        private Type abstractType;
        private Type concreteType;
        private TypeInfo concreteTypeInfo;
        private Lazy<ConstructorInfo> constructor;
        private Lazy<PropertyInfo[]> properties;
        private Lazy<FieldInfo[]> fields;
        private Lazy<MethodInfo[]> methods;
        private bool isThreadSafe = true;
        #endregion

        #region 初期化処理
        public ResolverConfig(Type abstractType, Type concreteType)
        {
            this.abstractType = abstractType;
            this.concreteType = concreteType;
            this.concreteTypeInfo = concreteType.GetTypeInfo();

            constructor = new Lazy<ConstructorInfo>(() => GetUsingConstructor(concreteTypeInfo), isThreadSafe);
            properties = new Lazy<PropertyInfo[]>(() => GetTargetProperties(concreteType), isThreadSafe);
            fields = new Lazy<FieldInfo[]>(() => GetTargetFields(concreteType), isThreadSafe);
            methods = new Lazy<MethodInfo[]>(() => GetTargetMethods(concreteType), isThreadSafe);
        }

        #endregion

        #region Private処理
        /// <summary>
        /// Get the metadata of the injected constructor.
        /// </summary>
        /// <param name="targetTypeInfo">Type infomation</param>
        private ConstructorInfo GetUsingConstructor(TypeInfo targetTypeInfo)
        {
            var ctors = targetTypeInfo.DeclaredConstructors.OrderByDescending(c => c.GetParameters().Length);
            var injectCtors = ctors.Where(c => c.GetCustomAttribute<InjectAttribute>() != null);

            if (injectCtors.Count() == 1)
            {
                return injectCtors.First();
            } else if (injectCtors.Count() == 0)
            {
                return ctors.FirstOrDefault();
            } else
            {
                throw new ContainerException("Multiple constructors can not be targeted for injection in the same class.");
            }
        }

        /// <summary>
        /// Get the metadata of the injected fields.
        /// </summary>
        /// <param name="targetType">Type infomation</param>
        private FieldInfo[] GetTargetFields(Type targetType)
        {
            return targetType.GetRuntimeFields().Where(f => f.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }

        /// <summary>
        /// Get the metadata of the injected properties.
        /// </summary>
        /// <param name="targetType">Type infomation</param>
        private PropertyInfo[] GetTargetProperties(Type targetType)
        {
            return targetType.GetRuntimeProperties().Where(p => p.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }

        /// <summary>
        /// Get the metadata of the injected methods.
        /// </summary>
        /// <param name="targetType">Type infomation</param>
        private MethodInfo[] GetTargetMethods(Type targetType)
        {
            return targetType.GetRuntimeMethods().Where(m => m.GetCustomAttribute<InjectAttribute>() != null).ToArray();
        }
        #endregion
    }
}
