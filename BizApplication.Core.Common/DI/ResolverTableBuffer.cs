using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.DI
{
    /// <summary>
    /// Provides ResolverConfig addition with less overhead.
    /// Since reference speed is slow, it should be used only when assembling ResolverTable.
    /// </summary>
    public class ResolverTableBuffer : IResolverTable
    {
        public ResolverTableBuffer()
        {
            dict = new Dictionary<Type, ResolverConfig>();
        }

        private IDictionary<Type, ResolverConfig> dict;
        private bool isCompiled;

        public bool IsCompiled
        {
            get
            {
                return isCompiled;
            }
        }


        public void Add(Type abstractType, ResolverConfig resolverConfig)
        {
            dict.Add(abstractType, resolverConfig);
        }

        [Obsolete("In order to improve the reference speed of ResolverConfig you should use Resolver's method")]
        public void Compile()
        {
            foreach (var pair in dict)
            {
                pair.Value.FactoryMethod = () => Activator.CreateInstance(pair.Value.ConcreteType);
            }
            isCompiled = true;
        }

        [Obsolete("In order to improve the reference speed of ResolverConfig you should use Resolver's method")]
        public ResolverConfig Get(Type abstractType)
        {
            return dict[abstractType];
        }

        [Obsolete("In order to improve the reference speed of ResolverConfig you should use Resolver's method")]
        public bool IsValid()
        {
            return true;
        }

        [Obsolete("In order to improve the reference speed of ResolverConfig you should use Resolver's method")]
        public int Count()
        {
            return dict.Count;
        }

        public IEnumerator<KeyValuePair<Type, ResolverConfig>> GetEnumerator()
        {
            foreach (var pair in dict)
            {
                yield return KeyValuePair.Create(pair.Key, pair.Value);
            }
        }
    }
}
