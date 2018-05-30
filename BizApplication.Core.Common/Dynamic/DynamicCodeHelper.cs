using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizApplication.Core.Common.Dynamic
{
    public sealed class DynamicCodeHelper : IDynamicCodeHelper
    {
        public DynamicCodeHelper()
        {
            _cachedMetaInfoDict = new Dictionary<string, ClassInfo>();
        }

        private IDictionary<string, ClassInfo> _cachedMetaInfoDict;

        public Type GetType(object target)
        {
            var type = target?.GetType();

            if (ReferenceEquals(type, null))
            {
                return null;
            }

            if (NoCache(type.AssemblyQualifiedName))
            {
                AddOrUpdateCache(type.AssemblyQualifiedName, new ClassInfo(type.Name));
            }

            return type;
        }

        public IList<Property> GetProperties(object target)
        {
            var type = GetType(target);

            if (ReferenceEquals(type, null))
            {
                return null;
            }

            var cache = GetCache(type.AssemblyQualifiedName);



            if (NoCacheProperties(cache))
            {
                cache.Properties = type.GetProperties()
                                       .Select(x => new Property(
                                           x.Name,
                                           x.PropertyType)).ToArray();
            }

            return cache.Properties;
        }

        public object GetPropertyValue(object target, string propertyName)
        {
            if (ReferenceEquals(target, null))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (ReferenceEquals(propertyName, null))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // TODO : ILで高速化する
            //var cache = GetProperties(target).FirstOrDefault(x => x.Name == propertyName);
            //return (target as dynamic)[cache.Name];

            return GetType(target).GetProperty(propertyName).GetValue(target);
        }

        public void SetPropertyValue(object target, string propertyName, object value)
        {
            if (ReferenceEquals(target, null))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (ReferenceEquals(propertyName, null))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // TODO : ILで高速化する
            //var cache = GetProperties(target).FirstOrDefault(x => x.Name == propertyName);
            //(target as dynamic)[cache.Name] = value;

            GetType(target).GetProperty(propertyName).SetValue(target, value);
        }

        public object CreateInstance(Type type)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }
            return Activator.CreateInstance(type);
        }

        public object CreateInstance(Type type, params object[] args)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Activator.CreateInstance(type, args);
        }

        public T CreateInstance<T>() where T : class
        {
            return (T)CreateInstance(typeof(T));
        }

        public T CreateInstance<T>(params object[] args) where T : class
        {
            return (T)CreateInstance(typeof(T), args);
        }

        private ClassInfo GetCache(string className)
        {
            if (className is null)
            {
                throw new ArgumentNullException("Argument is null");
            }
            
            if (NoCache(className))
            {
                throw new ArgumentNullException("There is not cache");
            }

            return _cachedMetaInfoDict[className];
        }

        private void AddOrUpdateCache(string className, ClassInfo cachedMetaInfo)
        {
            if (className is null)
            {
                throw new ArgumentNullException($"Argument is null");
            }

            if (cachedMetaInfo is null)
            {
                throw new ArgumentNullException($"Argument is null");
            }

            _cachedMetaInfoDict.Add(className, cachedMetaInfo);
        }

        private void RemoveCache(string className)
        {
            if (className is null)
            {
                throw new ArgumentNullException($"Argument is null");
            }
            _cachedMetaInfoDict.Remove(className);
        }

        /// <summary>
        /// Determine if the meta info is not cached
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>Returns true if it is not cached, false otherwise</returns>
        private bool NoCache(string className)
        {
            return !_cachedMetaInfoDict.ContainsKey(className);
        }

        /// <summary>
        /// Determine if the properties are not cached
        /// </summary>
        /// <param name="cachedMetaInfo">Cached meta info</param>
        /// <returns>Returns true if it is not cached, false otherwise</returns>
        private bool NoCacheProperties(ClassInfo cachedMetaInfo)
        {
            return ReferenceEquals(cachedMetaInfo.Properties, null);
        }

        private class ClassInfo
        {
            public ClassInfo(string className)
            {
                Name = className;
            }

            public string Name { get; set; }
            public IList<Property> Properties { get; set; }
        }
    }
}
