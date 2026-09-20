using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Object not found") : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
