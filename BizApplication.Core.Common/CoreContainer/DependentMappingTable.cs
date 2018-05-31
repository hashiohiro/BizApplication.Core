using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizApplication.Core.Common.CoreContainer
{
    /// <summary>
    /// A table that maps abstract types and dependencies.
    /// </summary>
    public sealed class DependentMappingTable
    {
        public DependentMappingTable()
        {
            _innerTable = new Dictionary<string, IList<DependencyMapping>>();
            CanRegister = true;
        }

        public bool CanRegister { get; private set; }

        private IDictionary<string, IList<DependencyMapping>> _innerTable;

        public DependencyMapping GetDependencyMapping(string abstractTypeName)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependencies(abstractTypeName).FirstOrDefault();          
        }

        public DependencyMapping GetDependencyMapping(string abstractTypeName, string concreteTypeName)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependencies(abstractTypeName).FirstOrDefault(x => x.ConcreteTypeName == concreteTypeName);          
        }

        public DependencyMapping GetDependencyMapping(string abstractTypeName, int resolvePriority = 0)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependencies(abstractTypeName).FirstOrDefault(x => x.ResolvePriority == resolvePriority);          
        }

        public void AddDependency(string abstractTypeName, string concreteTypeName, CoreContainerObjectLifeTimes objectLifeTime, int resolvePriority = 0)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be added after mapping table is built");
            }

            var d = new DependencyMapping(abstractTypeName, concreteTypeName, objectLifeTime, resolvePriority);

            if (IsExistAbstractTypeName(abstractTypeName))
            {
                var dependencies = GetDependencies(abstractTypeName);
                dependencies.Add(d);
                dependencies = dependencies.OrderByDescending(x => x.ResolvePriority).ToList();
            } else
            {
                AddAbstractType(abstractTypeName, d);
            }
        }

        public void RemoveDependency(string abstractTypeName, string concreteTypeName)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be removed after mapping table is built");
            }

            if (!IsExistAbstractTypeName(abstractTypeName))
            {
                throw new InvalidOperationException("No Exist abstract type");
            }

            var dependencies = GetDependencies(abstractTypeName);
            dependencies.Remove(GetDependencyMapping(abstractTypeName, concreteTypeName));
        }

        public void RemoveDependency(string abstractTypeName, int resolvePriority = 0)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be removed after mapping table is built");
            }

            if (!IsExistAbstractTypeName(abstractTypeName))
            {
                throw new InvalidOperationException("No Exist abstract type");
            }

            var dependencies = GetDependencies(abstractTypeName);
            dependencies.Remove(GetDependencyMapping(abstractTypeName, resolvePriority));
        }

        public void Build()
        {
            var dict = new Dictionary<string, IList<DependencyMapping>>();

            foreach (var m in _innerTable)
            {
                dict[m.Key] = _innerTable[m.Key].OrderByDescending(x => x.ResolvePriority).ToList();          
            }

            _innerTable = dict;

            CanRegister = false;
        }

        /// <summary>
        /// Determine whether an abstract type exists.
        /// </summary>
        /// <param name="abstractTypeName">Abstract type name</param>
        /// <returns>Returns true if an abstract type exists, false otherwise</returns>
        private bool IsExistAbstractTypeName(string abstractTypeName)
        {
            return _innerTable.ContainsKey(abstractTypeName) && _innerTable[abstractTypeName].Count > 0;
        }

        /// <summary>
        /// Determine whether an concrete type exists.
        /// </summary>
        /// <param name="abstractTypeName">Concrete type name</param>
        /// <returns>Returns true if an concrete type exists, false otherwise</returns>
        private bool IsExistConcreteTypeName(string abstractTypeName, string concreteTypeName)
        {
            var dependencies = GetDependencies(abstractTypeName);
            return dependencies.Any(x => x.ConcreteTypeName == concreteTypeName);
        }

        /// <summary>
        /// Get dependency corresponding to abstract type.
        /// </summary>
        /// <param name="abstractTypeName">Abstract type name</param>
        /// <returns>Dependency list</returns>
        private IList<DependencyMapping> GetDependencies(string abstractTypeName)
        {
            return _innerTable[abstractTypeName];
        }

        /// <summary>
        /// Add an abstract type.
        /// </summary>
        /// <param name="abstractTypeName">Abstract type name</param>
        /// <param name="dependency">Dependency</param>
        private void AddAbstractType(string abstractTypeName, DependencyMapping dependency)
        {
            _innerTable[abstractTypeName] = new List<DependencyMapping>() { dependency };
        }

        /// <summary>
        /// Determine whether construction of the mapping table has been completed.
        /// </summary>
        /// <returns></returns>
        private bool IsCompletedConstructionMappingTable()
        {
            return !CanRegister;
        }
    }
    public class DependencyMapping
    {
        public DependencyMapping(string abstractTypeName, string concreteTypeName, CoreContainerObjectLifeTimes lifetime, int resolvePriority = 0)
        {
            AbstractTypeName = abstractTypeName;
            ConcreteTypeName = concreteTypeName;
            ObjectLifeTime = lifetime;
            ResolvePriority = resolvePriority;
        }

        public string AbstractTypeName { get; private set; }
        public string ConcreteTypeName { get; private set; }
        public CoreContainerObjectLifeTimes ObjectLifeTime { get; private set; }
        public int ResolvePriority { get; private set; }
    }
}
