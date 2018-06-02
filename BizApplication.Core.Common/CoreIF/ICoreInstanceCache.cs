using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreIF
{
    /// <summary>
    /// Provide a cache of instances.
    /// </summary>
    public interface ICoreInstanceCache
    {
        /// <summary>
        /// Add an instance.
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        /// <param name="instance">Instance</param>
        /// <param name="expirationDateTime">Expiration DateTime</param>
        void Add(Type instanceType, object instance, DateTimeOffset? expirationDateTime);

        /// <summary>
        /// Add an instance.
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="expirationTime">Expiration DateTime</param>
        void Add<TInstance>(TInstance instance, DateTimeOffset? expirationDateTime);

        /// <summary>
        /// Add or update an instance
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        /// <param name="instance">Instance</param>
        /// <param name="expirationDateTime">Expiration DateTime</param>
        void AddOrUpdate(Type instanceType, object instance, DateTimeOffset? expirationDateTime);

        /// <summary>
        /// Add or update an instance
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="expirationTime">Expiration DateTime</param>
        void AddOrUpdate<TInstance>(TInstance instance, DateTimeOffset? expirationDateTime);

        /// <summary>
        /// Get an instance.
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        /// <returns>Instance</returns>
        object Get(Type instanceType);

        /// <summary>
        /// Get an instance.
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        /// <returns>Instance</returns>
        object Get<TInstance>();
        
        /// <summary>
        /// Get or update an instance.
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        /// <param name="instance">Instance</param>
        /// <param name="expirationTime">Expiration DateTime</param>
        /// <returns>Instance</returns>
        object GetOrUpdate(Type instanceType, object instance, DateTimeOffset? expirationDateTime);
        
        /// <summary>
        /// Get or update an instance.
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="expirationTime">Expiration DateTime</param>
        /// <returns></returns>
        object GetOrUpdate<TInstance>(object instance, DateTimeOffset? expirationDateTime);

        /// <summary>
        /// Remove an instance.
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        void Remove(Type instanceType);

        /// <summary>
        /// Remove an instance.
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        void Remove<TInstance>();

        /// <summary>
        /// Determine whether the cache has been expired.
        /// </summary>
        /// <param name="instanceType">Instance type</param>
        /// <returns>Returns true if the cache has been expired, false otherwise</returns>
        bool HasExpired(Type instanceType);

        /// <summary>
        /// Determine whether the cache has been expired.
        /// </summary>
        /// <typeparam name="TInstance">Instance type</typeparam>
        /// <returns>Determine whether the cache has been expired</returns>
        bool HasExpired<TInstance>();
    }
}
