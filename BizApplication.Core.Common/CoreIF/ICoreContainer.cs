using BizApplication.Core.Common.CoreContainer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BizApplication.Core.Common.CoreIF
{
    /// <summary>
    /// Provide dependency injection.
    /// </summary>
    public interface ICoreContainer
    {
        /// <summary>
        /// Register correspondence between abstract type and concrete type.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <param name="concreteType">Concrete type</param>
        /// <param name="lifeTimes">Life time</param>
        /// <param name="priority">Resolve Priority</param>
        void Register(Type abstractType, Type concreteType, ObjectLifeTimes lifeTimes, int priority = 0);

        /// <summary>
        /// Register correspondence between abstract type and concrete type.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract type</typeparam>
        /// <typeparam name="TConcrete">Concrete type</typeparam>
        /// <param name="lifeTimes">Object Life time</param>
        /// <param name="priority">Resolve Priority</param>
        void Register<TAbstract, TConcrete>(ObjectLifeTimes lifeTimes, int priority = 0)
            where TAbstract : class
            where TConcrete : class;

        /// <summary>
        /// Register correspondence between abstract type and concrete type based on Export attribute.
        /// </summary>
        /// <param name="baseSearchAssembly">Base search assembly</param>
        /// <param name="attributeType">Attribute Type</param>
        void RegisterByCustomAttribute(Assembly baseSearchAssembly, Type attributeType);

        /// <summary>
        /// Register correspondence between abstract type and concrete type based on Export attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute Type<</typeparam>
        /// <param name="baseSearchAssembly">Base search assembly</param>
        void RegisterByCustomAttributes<TAttribute>(Assembly baseSearchAssembly) where TAttribute : Attribute;

        /// <summary>
        /// Resolve a concrete type from the abstract type.
        /// </summary>
        /// <param name="Abstract type">Abstract type</param>
        /// <param name="args">Arguments</param>
        /// <returns>Instance</returns>
        object Resolve(Type abstractType, params object[] args);

        /// <summary>
        /// Resolve a concrete type from the abstract type.
        /// </summary>
        /// <typeparam name="TAbstract">Abstract Type</typeparam>
        /// <param name="args">Arguments</param>
        /// <returns>Instance</returns>
        TAbstract Resolve<TAbstract>(params object[] args) where TAbstract : class;

        /// <summary>
        /// Construct dependency mapping table.
        /// You can not add new dependencies after construction.
        /// </summary>
        void Build();
    }

    public enum ObjectLifeTimes
    {
        // Always create new objects
        Transient,
        // Not implemented
        Scoped,
        // Always return same object
        Singleton,
    }
}
