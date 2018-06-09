using System;
using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Error;
using BizApplication.Core.Common.Util;

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
            return GetFactoryMethod(abstractType)();
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract type</typeparam>
        /// <returns>a dependency</returns>
        public TAbstract Resolve<TAbstract>()
        {
            return (TAbstract)GetFactoryMethod(typeof(TAbstract))();
        }

        /// <summary>
        /// (Lazy evaluation)Resolve a dependency.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <returns>Lazy instance</returns>
        public Lazy<object> LazyResolve(Type abstractType)
        {
            return new Lazy<object>(() => GetFactoryMethod(abstractType)());
        }

        /// <summary>
        /// (Lazy evaluation)Resolve a dependency.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract type</typeparam>
        /// <returns>Lazy instance</returns>
        public Lazy<TAbstract> LazyResolve<TAbstract>()
        {
            return new Lazy<TAbstract>(() => (TAbstract)GetFactoryMethod(typeof(TAbstract))());
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
        }

        /// <summary>
        /// Check that it is uncompiled.
        /// </summary>
        private void CheckNotCompiled()
        {
            if (resolverTable.IsCompiled)
            {
                throw new ContainerException("Resolver table has already been compiled.");
            }
        }

        /// <summary>
        /// Get a factory method
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <returns>Factory method</returns>
        private Func<object> GetFactoryMethod(Type abstractType)
        {
            AssertUtil.AssertNotNull(abstractType);

            if (!resolverTable.IsCompiled)
            {
                throw new ContainerException("The Resolver Table must be precompiled");
            }

            return resolverTable.Get(abstractType).FactoryMethod;
        }
    }
}
