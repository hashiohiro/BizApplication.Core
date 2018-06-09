using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Error;
using BizApplication.Core.Common.Util;


namespace BizApplication.Core.Common.DI
{
    /// <summary>
    /// Provides ResolverConfig reference with low overhead.
    /// To continuously add ResolverConfig, use ResolverTableBuffer
    /// </summary>
    public class ResolverTable : IResolverTable
    {
        
        public ResolverTable(IResolverTable resolverTable, int tableSize)
        {
            _innerTable = new Hashtuple[tableSize][];
            foreach (var pair in resolverTable)
            {
                Add(pair.Key, pair.Value);
            }

            isCompiled = resolverTable.IsCompiled;
        }

        public bool IsCompiled
        {
            get
            {
                return isCompiled;
            }
        }

        private Hashtuple[][] _innerTable;
        private bool isCompiled;

        public void Add(Type abstractType, ResolverConfig resolverConfig)
        {
            var index = abstractType.GetHashCode() % _innerTable.Length;
            var buckets = _innerTable[index];
            if (AssertUtil.IsNull(buckets))
            {
                buckets = new Hashtuple[1];
                buckets[0] = new Hashtuple() { type = abstractType, resolverConfig = resolverConfig };
            } else
            {
                var newBuckets = new Hashtuple[buckets.Length + 1];
                Array.Copy(buckets, newBuckets, buckets.Length);
                buckets = newBuckets;
                buckets[buckets.Length - 1] = new Hashtuple() { type = abstractType, resolverConfig = resolverConfig };
            }
            _innerTable[index] = buckets;
        }

        public ResolverConfig Get(Type abstractType)
        {
            var buckets = _innerTable[abstractType.GetHashCode() % _innerTable.Length];
            for (var i = 0; i < buckets.Length; i++)
            {
                // TODO: 参照先の比較で試す。もしかすると値レベルの比較が必要かもしれない。
                if (ReferenceEquals(buckets[i].type, abstractType))
                {
                    return buckets[i].resolverConfig;
                }
            }

            throw new ContainerException($"Dependency is not registered for abstract type [AbstractType] : { abstractType.Name }");
        }

        // TODO : エラー個所とエラー内容が分かるようにする
        public bool IsValid()
        {
            foreach (var ts in _innerTable)
            {
                if (AssertUtil.IsNull(ts))
                {
                    continue;
                }
                foreach (var t in ts)
                {
                    if (AssertUtil.IsNull(t.resolverConfig.AbstractType))
                    {
                        return false;
                    }

                    if (AssertUtil.IsNull(t.resolverConfig.ConcreteType))
                    {
                        return false;
                    }

                    if (t.resolverConfig.ConcreteType.IsPrimitive)
                    {
                        return false;
                    }

                    if (!t.resolverConfig.AbstractType.IsClass && !t.resolverConfig.AbstractType.IsInterface)
                    {
                        return false;
                    }

                    if (!t.resolverConfig.ConcreteType.IsClass)
                    {
                        return false;
                    }

                    if (!ReferenceEquals(t.resolverConfig.AbstractType, t.resolverConfig.ConcreteType))
                    {
                        if (!t.resolverConfig.ConcreteType.IsSubclassOf(t.resolverConfig.AbstractType))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void Compile()
        {
            foreach (var ts in _innerTable)
            {
                if (AssertUtil.IsNull(ts))
                {
                    continue;
                }

                foreach (var t in ts)
                {
                    var factoryMethod = new DynamicMethod("FactoryMethod", typeof(object), new Type[] { });
                    var il = factoryMethod.GetILGenerator();
                    EmitNewObj(t.resolverConfig, il);
                    il.Emit(OpCodes.Ret);
                    t.resolverConfig.FactoryMethod = (Func<object>)factoryMethod.CreateDelegate(typeof(Func<object>));
                }
            }
            isCompiled = true;
        }

        private void EmitNewObj(ResolverConfig resolverConfig, ILGenerator il)
        {
            var ctorParams = resolverConfig.Constructor.GetParameters();
            {
                foreach (var p in ctorParams)
                {
                    var pRes = Get(p.ParameterType);
                    EmitNewObj(pRes, il);
                }
                il.Emit(OpCodes.Newobj, resolverConfig.Constructor);
            }
            {
                foreach (var f in resolverConfig.Fields)
                {
                    il.Emit(OpCodes.Dup);
                    var fRes = Get(f.FieldType);
                    EmitNewObj(fRes, il);
                    il.Emit(OpCodes.Stfld, f);
                }
            }
            {
                foreach (var p in resolverConfig.Properties)
                {
                    il.Emit(OpCodes.Dup);
                    var pRes = Get(p.PropertyType);
                    EmitNewObj(pRes, il);
                    EmitCall(p.SetMethod, il);
                }
            }
            {
                foreach (var m in resolverConfig.Methods)
                {
                    il.Emit(OpCodes.Dup);
                    var mParams = m.GetParameters();
                    foreach (var mp in mParams)
                    {
                        var mpRes = Get(mp.ParameterType);
                        EmitNewObj(mpRes, il);
                    }
                    EmitCall(m, il);
                }
            }
        }

        private void EmitCall(MethodInfo method, ILGenerator il)
        {
            if (method.IsFinal || !method.IsVirtual)
            {
                il.Emit(OpCodes.Call, method);
            } else
            {
                il.Emit(OpCodes.Callvirt, method);
            }
        }

        public int Count()
        {
            var c = 0;
            foreach(var ts in _innerTable)
            {
                foreach (var t in ts)
                {
                    c++;
                }
            }
            return c;
        }

        public IEnumerator<KeyValuePair<Type, ResolverConfig>> GetEnumerator()
        {
            foreach (var ts in _innerTable)
            {
                foreach (var t in ts)
                {
                    yield return KeyValuePair.Create(t.type, t.resolverConfig);
                }
            }
        }
    }
    public struct Hashtuple
    {
        public Type type;
        public ResolverConfig resolverConfig;
    }
}
