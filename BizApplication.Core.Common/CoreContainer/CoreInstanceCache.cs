using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreContainer
{
    public class CoreInstanceCache : ICoreInstanceCache
    {
        public CoreInstanceCache(IDateTimeService dateTimeService)
        {
            _cacheContainer = new Dictionary<string, InstanceCacheInfo>();
            _dateTimeService = dateTimeService;
        }

        private IDictionary<string, InstanceCacheInfo> _cacheContainer;
        private IDateTimeService _dateTimeService;
        
        public void Add(Type instanceType, object instance, DateTimeOffset? expirationDateTime)
        {
            if (ReferenceEquals(instanceType, null))
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            if (ReferenceEquals(instance, null))
            {
                throw new ArgumentNullException(nameof(instance));
            }

            AddInstance(GetContainerKey(instanceType), instance, expirationDateTime);
        }

        public void Add<TInstance>(TInstance instance, DateTimeOffset? expirationDateTime)
        {
            Add(typeof(TInstance), instance, expirationDateTime);
        }

        public void AddOrUpdate(Type instanceType, object instance, DateTimeOffset? expirationDateTime)
        {
            if (ReferenceEquals(instanceType, null))
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            if (ReferenceEquals(instance, null))
            {
                throw new ArgumentNullException(nameof(instance));
            }

            AddOrUpdateInstance(GetContainerKey(instanceType), instance, expirationDateTime);
        }

        public void AddOrUpdate<TInstance>(TInstance instance, DateTimeOffset? expirationDateTime)
        {
            AddOrUpdate(typeof(TInstance), instance, expirationDateTime);
        }

        public object Get(Type instanceType)
        {
            if (ReferenceEquals(instanceType, null))
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            return GetInstance(GetContainerKey(instanceType));
        }

        public object Get<TInstance>()
        {
            return Get(typeof(TInstance));
        }

        public object GetOrUpdate(Type instanceType, object instance, DateTimeOffset? expirationDateTime)
        {
            if (ReferenceEquals(instanceType, null))
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            if (ReferenceEquals(instance, null))
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (IsExpiredInstance(GetContainerKey(instanceType)))
            {
                AddOrUpdate(instanceType, instance, expirationDateTime);
            }

            return GetInstance(GetContainerKey(instanceType));
        }

        public object GetOrUpdate<TInstance>(object instance, DateTimeOffset? expirationDateTime)
        {
            return GetOrUpdate(typeof(TInstance), instance, expirationDateTime);
        }

        public void Remove(Type instanceType)
        {
            if (ReferenceEquals(instanceType, null))
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            RemoveInstance(GetContainerKey(instanceType));
        }

        public void Remove<TInstance>()
        {
            Remove(typeof(TInstance));
        }

        public bool HasExpired(Type instanceType)
        {
            return IsExpiredInstance(GetContainerKey(instanceType));
        }

        public bool HasExpired<TInstance>()
        {
            return HasExpired(typeof(TInstance));
        }

        private InstanceCacheInfo GetInstanceCacheInfo(string key)
        {
            if (NoContainsKey(key))
            {
                throw new InvalidOperationException("No instance of such a type exists");
            }

            return _cacheContainer[key];
        }
        /// <summary>
        /// Get the key of the cache container.
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>Key</returns>
        private string GetContainerKey(Type t)
        {
            return t.AssemblyQualifiedName;
        }

        /// <summary>
        /// Determine if the same key already exists
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns true if the same key exists, false otherwise.</returns>
        private bool ContainsKey(string key)
        {
            return _cacheContainer.ContainsKey(key);
        }

        /// <summary>
        /// Determine whether the same key does not exist
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns true if the same key does not exist, false otherwise</returns>
        private bool NoContainsKey(string key)
        {
            return !ContainsKey(key);
        }

        /// <summary>
        /// Add an instance to the cache container.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="instance">Instance</param>
        /// <param name="expirationDateTime">Expiration time</param>
        private void AddInstance(string key, object instance, DateTimeOffset? expirationDateTime)
        {
            if (ContainsKey(key))
            {
                throw new InvalidOperationException("Duplicate key.");
            }

            AddOrUpdateInstance(key, instance, expirationDateTime);
        }

        /// <summary>
        /// Add or update an instance to the cache container.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="instance">Instance</param>
        /// <param name="expirationDateTime">Expiration time</param>
        private void AddOrUpdateInstance(string key, object instance, DateTimeOffset? expirationDateTime)
        {
            _cacheContainer[key] = CreateInstanceCacheInfo(instance, expirationDateTime);
        }

        /// <summary>
        /// Get an instance from the cache container.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Instance</returns>
        private object GetInstance(string key)
        {
            return GetInstanceCacheInfo(key).Instance;
        }

        private object GetOrUpdateInstance(string key, object instance, DateTimeOffset? expirationDateTime)
        {
            var cacheInfo = GetInstanceCacheInfo(key);



            return cacheInfo.Instance;
        }

        private bool IsExpiredInstance(string key)
        {
            var cacheInfo = GetInstanceCacheInfo(key);

            if (ReferenceEquals(cacheInfo.ExpirationDateTime, null ))
            {
                return false;
            }

            return cacheInfo.ExpirationDateTime >= _dateTimeService.GetLocalNow();
        }

        /// <summary>
        /// Delete the instance from the cache container.
        /// </summary>
        /// <param name="key">Key</param>
        private void RemoveInstance(string key)
        {
            if (NoContainsKey(key))
            {
                throw new InvalidOperationException("No instance of such a type exists");
            }
            _cacheContainer.Remove(key);
        }

        private InstanceCacheInfo CreateInstanceCacheInfo(object instance, DateTimeOffset? expirationDateTime)
        {
            return new InstanceCacheInfo()
            {
                Instance = instance,
                ExpirationDateTime = expirationDateTime,
            };

        }

        private class InstanceCacheInfo
        {
            public object Instance { get; set; }
            public DateTimeOffset? ExpirationDateTime { get; set; }
        }
    }
}
