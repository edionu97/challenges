using System;

namespace CelestialObjectCatalog.Persistence.Exceptions
{
    public class EntityAlreadyExistsException : ArgumentException
    {
        public EntityAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
