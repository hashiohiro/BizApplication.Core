using BizApplication.Core.Common.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreIF
{
    public interface IResolverTable
    {
        /// <summary>
        /// Add a resolve configuration.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <param name="resolverConfig">Resolver configuration</param>
        void Add(Type abstractType, ResolverConfig resolverConfig);

        /// <summary>
        /// Validate all resolver configurations.
        /// </summary>
        /// <returns>Returns true if the validity is guaranteed, false otherwise</returns>
        bool IsValid();

        /// <summary>
        /// Get a resolver configurations.
        /// </summary>
        /// <param name="abstractType">Abstract type</param>
        /// <returns>Resolver configuration</returns>
        ResolverConfig Get(Type abstractType);

        /// <summary>
        /// Get the number of resolver configurations registered.
        /// </summary>
        /// <returns>the number of resolver configurations registered</returns>
        int Count();

        /// <summary>
        /// Generate dynamic code
        /// </summary>
        /// <remarks>
        /// Dynamically generate factory methods etc. based on Resolver table.
        /// </remarks>
        void Compile();

        IEnumerator<KeyValuePair<Type, ResolverConfig>> GetEnumerator();
    }
}
