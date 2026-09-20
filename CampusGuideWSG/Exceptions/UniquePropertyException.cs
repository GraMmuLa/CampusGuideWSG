namespace CampusGuideWSG.Exceptions
{
    public class UniquePropertyException : Exception
    {
        public UniquePropertyException(string message = "Instance with this property value already exists")
            : base(message)
        { }
        public UniquePropertyException(Exception innerException,
            string message = "Instance with this property value already exists")
    : base(message, innerException)
        { }
    }
}
