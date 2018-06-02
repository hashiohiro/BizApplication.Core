using BizApplication.Core.Common.CoreIF;

namespace BizApplication.Core.Common.CoreContainer
{
    public class DependencyMapping
    {
        public DependencyMapping(string abstractTypeName, string concreteTypeName, ObjectLifeTimes lifetime, int resolvePriority = 0)
        {
            AbstractTypeName = abstractTypeName;
            ConcreteTypeName = concreteTypeName;
            ObjectLifeTime = lifetime;
            ResolvePriority = resolvePriority;
        }

        public string AbstractTypeName { get; private set; }
        public string ConcreteTypeName { get; private set; }
        public ObjectLifeTimes ObjectLifeTime { get; private set; }
        public int ResolvePriority { get; private set; }
    }
}
