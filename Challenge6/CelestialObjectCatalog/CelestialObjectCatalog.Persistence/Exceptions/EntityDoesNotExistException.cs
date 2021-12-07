using System;

namespace CelestialObjectCatalog.Persistence.Exceptions
{
    public class EntityDoesNotExistException : ArgumentException
    {
        public EntityDoesNotExistException(string message) : base(message)
        {
        }
    }
}
