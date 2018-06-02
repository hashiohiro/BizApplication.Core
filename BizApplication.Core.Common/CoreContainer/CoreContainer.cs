using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BizApplication.Core.Common.CoreContainer
{
    public class CoreContainer : ICoreContainer
    {
        public CoreContainer(ICoreInstanceCache coreInstanceCache, IDynamicCodeHelper dynamicCodeHelper)
        {
            mappingTable = new DependencyMappingTable();
            this.coreInstanceCache = coreInstanceCache;
            this.dynamicCodeHelper = dynamicCodeHelper;
        }

        private DependencyMappingTable mappingTable;
        private ICoreInstanceCache coreInstanceCache;
        private IDynamicCodeHelper dynamicCodeHelper;

        public void Register(Type abstractType, Type concreteType, ObjectLifeTimes objectLifeTime, int priority = 0)
        {
            RegisterDependencyMapping(abstractType, concreteType, objectLifeTime, priority);
        }

        public void Register<TAbstract, TConcrete>(ObjectLifeTimes lifeTimes, int priority = 0)
            where TAbstract : class
            where TConcrete : class
        {
            Register(typeof(TAbstract), typeof(TConcrete), lifeTimes, priority);
        }

        public void RegisterByCustomAttribute(Assembly baseSearchAssembly, Type attributeType)
        {
            if (attributeType.IsSubclassOf(typeof(RegisterTypeAttribute)))
            {
                throw new InvalidOperationException("Attribute must inherit RegisterTypeAttribute.");
            }

            foreach (var type in baseSearchAssembly.GetTypes())
            {
                var attr = (RegisterTypeAttribute)type.GetCustomAttribute(attributeType);

                if (ReferenceEquals(attr, null))
                {
                    continue;
                }

                Register(attr.AbstractType, type, attr.ObjectLifeTime, attr.Priority);
            }
        }

        public void RegisterByCustomAttributes<TAttribute>(Assembly baseSearchAssembly) where TAttribute : Attribute
        {
            RegisterByCustomAttribute(baseSearchAssembly, typeof(TAttribute));
        }

        public object Resolve(Type abstractType, params object[] args)
        {
            var dm = mappingTable.GetDependencyMapping(abstractType.AssemblyQualifiedName);

            if (dm.ObjectLifeTime == ObjectLifeTimes.Singleton)
            {
                return GetSingletonInstance(Type.GetType(dm.ConcreteTypeName));
            } else
            {
                return CreateInstance(Type.GetType(dm.ConcreteTypeName), args);
            }
        }

        public TAbstract Resolve<TAbstract>(params object[] args) where TAbstract : class
        {
            return (TAbstract)Resolve(typeof(TAbstract), args);
        }

        public void Build()
        {
            mappingTable.Build();
        }

        private object CreateInstance(Type instanceType, params object[] args)
        {
            if (args.Length == 0)
            {
                return dynamicCodeHelper.CreateInstance(instanceType);
            } else
            {
                return dynamicCodeHelper.CreateInstance(instanceType, args);
            }
        }

        private object GetSingletonInstance(Type instanceType)
        {
            return coreInstanceCache.Get(instanceType);
        }

        private void RegisterDependencyMapping(Type abstractType, Type concreteType, ObjectLifeTimes objectLifeTime, int priority)
        {
            if (ReferenceEquals(abstractType, null))
            {
                throw new ArgumentNullException(nameof(abstractType));
            }

            if (ReferenceEquals(concreteType, null))
            {
                throw new ArgumentNullException(nameof(concreteType));
            }

            mappingTable.AddDependency(abstractType.AssemblyQualifiedName, concreteType.AssemblyQualifiedName, objectLifeTime, priority);
        }

        private void RegisterInstanceCache(Type instanceType, object[] args, ObjectLifeTimes objectLifeTime)
        {
            if (objectLifeTime != ObjectLifeTimes.Singleton)
            {
                return;
            }

            coreInstanceCache.Add(instanceType, CreateInstance(instanceType, args), null);
        }
    }
}
