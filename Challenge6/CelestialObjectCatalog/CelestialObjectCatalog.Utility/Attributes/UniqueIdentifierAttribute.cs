using System;
using System.Runtime.CompilerServices;

namespace CelestialObjectCatalog.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class UniqueIdentifierAttribute : Attribute
    {
        public int Order { get; }

        public UniqueIdentifierAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }
    }
}
