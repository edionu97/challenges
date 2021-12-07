namespace CelestialObjectCatalog.Persistence.Constants
{
    public static class MessageConstants
    {
        public static string EntityCouldNotBeDeletedMessage =>
            "Entity could not be deleted, reason it does not exist";

        public static string EntityCouldNotBeUpdatedMessage =>
            EntityCouldNotBeDeletedMessage.Replace("deleted", "update");

        public static string UniqueIdsCannotBeComputedMessage =>
            "The entity does not have any property annotated with [UniqueIdentificationAttribute] or not all properties can be converted to {0} value. ";
    }
}
