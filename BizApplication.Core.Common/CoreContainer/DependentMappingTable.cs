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
            _innerTable = new Dictionary<string, IList<DependentMapping>>();
            CanRegister = true;
        }

        public bool CanRegister { get; private set; }

        private IDictionary<string, IList<DependentMapping>> _innerTable;

        public DependentMapping GetDependentMapping(string abstractTypeName)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependents(abstractTypeName).FirstOrDefault();          
        }

        public DependentMapping GetDependentMapping(string abstractTypeName, string concreteTypeName)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependents(abstractTypeName).FirstOrDefault(x => x.ConcreteTypeName == concreteTypeName);          
        }

        public DependentMapping GetDependentMapping(string abstractTypeName, int resolvePriority = 0)
        {
            if (!IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("It is necessary to build the mapping table");
            }
            return GetDependents(abstractTypeName).FirstOrDefault(x => x.ResolvePriority == resolvePriority);          
        }

        public void AddDependent(string abstractTypeName, string concreteTypeName, CoreContainerObjectLifeTimes objectLifeTime, int resolvePriority = 0)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be added after mapping table is built");
            }

            var d = new DependentMapping(abstractTypeName, concreteTypeName, objectLifeTime, resolvePriority);

            if (IsExistAbstractTypeName(abstractTypeName))
            {
                var dependents = GetDependents(abstractTypeName);
                dependents.Add(d);
                dependents = dependents.OrderByDescending(x => x.ResolvePriority).ToList();
            } else
            {
                AddAbstractType(abstractTypeName, d);
            }
        }

        public void RemoveDependent(string abstractTypeName, string concreteTypeName)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be removed after mapping table is built");
            }

            if (!IsExistAbstractTypeName(abstractTypeName))
            {
                throw new InvalidOperationException("No Exist abstract type");
            }

            var dependents = GetDependents(abstractTypeName);
            dependents.Remove(GetDependentMapping(abstractTypeName, concreteTypeName));
        }

        public void RemoveDependent(string abstractTypeName, int resolvePriority = 0)
        {
            if (IsCompletedConstructionMappingTable())
            {
                throw new InvalidOperationException("Mapping can not be removed after mapping table is built");
            }

            if (!IsExistAbstractTypeName(abstractTypeName))
            {
                throw new InvalidOperationException("No Exist abstract type");
            }

            var dependents = GetDependents(abstractTypeName);
            dependents.Remove(GetDependentMapping(abstractTypeName, resolvePriority));
        }

        public void Build()
        {
            var dict = new Dictionary<string, IList<DependentMapping>>();

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
            var dependents = GetDependents(abstractTypeName);
            return dependents.Any(x => x.ConcreteTypeName == concreteTypeName);
        }

        /// <summary>
        /// Get dependency corresponding to abstract type.
        /// </summary>
        /// <param name="abstractTypeName">Abstract type name</param>
        /// <returns>Dependency list</returns>
        private IList<DependentMapping> GetDependents(string abstractTypeName)
        {
            return _innerTable[abstractTypeName];
        }

        /// <summary>
        /// Add an abstract type.
        /// </summary>
        /// <param name="abstractTypeName">Abstract type name</param>
        /// <param name="dependent">Dependent</param>
        private void AddAbstractType(string abstractTypeName, DependentMapping dependent)
        {
            _innerTable[abstractTypeName] = new List<DependentMapping>() { dependent };
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
    public class DependentMapping
    {
        public DependentMapping(string abstractTypeName, string concreteTypeName, CoreContainerObjectLifeTimes lifetime, int resolvePriority = 0)
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
