namespace CampusGuideWSG.Exceptions
{
    public class MissingDataException : Exception
    {
        public MissingDataException(string message = "Missing field") : base(message)
        { }

        public MissingDataException(Exception innerException,
            string message = "Missing field") : base(message, innerException)
        { }
    }
}
