using BizApplication.Core.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreIF
{
    /// <summary>
    /// Provides helper methods to use in dynamic code.
    /// </summary>
    public interface IDynamicCodeHelper
    {
        /// <summary>
        /// Get a type object
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Type object</returns>
        Type GetType(object target);

        /// <summary>
        /// Gets all public properties.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Property list</returns>
        IList<Property> GetProperties(object target);

        /// <summary>
        /// Get a property value.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value</returns>
        object GetPropertyValue(object target, string propertyName);

        /// <summary>
        /// Sets a property value.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        void SetPropertyValue(object target, string propertyName, object value);

        /// <summary>
        /// Dynamically instantiate
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Instance</returns>
        object CreateInstance(Type type);

        /// <summary>
        /// Dynamically instantiate
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="args">Arguments</param>
        /// <returns>Instance</returns>
        object CreateInstance(Type type, params object[] args);

        /// <summary>
        /// Dynamically instantiate
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Instance</returns>
        T CreateInstance<T>() where T : class;

        /// <summary>
        /// Dynamically instantiate
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="args">Arguments</param>
        /// <returns>Instance</returns>
        T CreateInstance<T>(params object[] args) where T : class;
    }
}
