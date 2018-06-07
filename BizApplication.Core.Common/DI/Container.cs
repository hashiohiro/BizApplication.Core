using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Error;
using BizApplication.Core.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace BizApplication.Core.Common.DI
{
    /// <summary>
    /// Provide dependency injection.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Get instance of container
        /// </summary>
        public Container()
        {
            this.resolverTable = new ResolverTableBuffer();
        }

        /// <summary>
        /// Get instance of container
        /// </summary>
        /// <param name="resolverTable">Resolver table</param>
        public Container(IResolverTable resolverTable)
        {
            this.resolverTable = resolverTable;
        }
        private IResolverTable resolverTable;
        private bool isCompiled;

        /// <summary>
        /// Register the dependency.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <param name="concreteType">Concrete type</param>
        public void Register(Type abstractType, Type concreteType)
        {
            CheckNotCompiled();

            AssertUtil.AssertNotNull(abstractType);
            AssertUtil.AssertNotNull(concreteType);

            resolverTable.Add(abstractType, new ResolverConfig(abstractType, concreteType));
        }

        /// <summary>
        /// Register the dependency.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract type</typeparam>
        /// <typeparam name="TConcrete">Concrete type</typeparam>
        public void Register<TAbstract, TConcrete>() where TAbstract : class where TConcrete : class
        {
            Register(typeof(TAbstract), typeof(TConcrete));
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <returns>a dependency</returns>
        public object Resolve(Type abstractType)
        {
            AssertUtil.AssertNotNull(abstractType);

            if (!isCompiled)
            {
                throw new ContainerException("The Resolver Table must be precompiled");
            }

            return resolverTable.Get(abstractType).FactoryMethod();
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract type</typeparam>
        /// <returns>a dependency</returns>
        public TAbstract Resolve<TAbstract>()
        {
            return (TAbstract)Resolve(typeof(TAbstract));
        }

        /// <summary>
        /// Compile the Resolver table.
        /// </summary>
        public void Compile()
        {
            CheckNotCompiled();

            resolverTable = new ResolverTable(resolverTable, resolverTable.Count());

            if (!resolverTable.IsValid())
            {
                throw new ContainerException("Unusual settings found in Resolver table.");
            }
            resolverTable.Compile();
            isCompiled = true;
        }

        /// <summary>
        /// Check that it is uncompiled.
        /// </summary>
        private void CheckNotCompiled()
        {
            if (isCompiled)
            {
                throw new ContainerException("Resolver table has already been compiled.");
            }
        }
    }
}
