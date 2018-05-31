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
        public CoreContainer(IDynamicCodeHelper dynamicCodeHelper)
        {
            mappingTable = new DependentMappingTable();
            this.dynamicCodeHelper = dynamicCodeHelper;
        }

        private DependentMappingTable mappingTable;
        private IDynamicCodeHelper dynamicCodeHelper;

        public void Register(Type abstractType, Type concreteType, CoreContainerObjectLifeTimes objectLifeTime, int priority = 0)
        {
            mappingTable.AddDependency(abstractType.AssemblyQualifiedName, concreteType.AssemblyQualifiedName, objectLifeTime, priority);
        }

        public void Register<TAbstract, TConcrete>(CoreContainerObjectLifeTimes lifeTimes, int priority = 0)
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

        public void RegisterByCustomAttributes<TAttribute>(CoreContainerObjectLifeTimes lifeTimes, int priority = 0) where TAttribute : Attribute
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type abstractType, params object[] args)
        {
            var dm = mappingTable.GetDependencyMapping(abstractType.AssemblyQualifiedName);

            if (args.Length == 0)
            {
                return dynamicCodeHelper.CreateInstance(Type.GetType(dm.ConcreteTypeName));
            } else
            {
                return dynamicCodeHelper.CreateInstance(Type.GetType(dm.ConcreteTypeName), args);
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
    }
}
