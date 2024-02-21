namespace MainUnit.Models.Exceptions
{
    public class RoomExistsException : Exception
    {
        public RoomExistsException()
        {
        }

        public RoomExistsException(string? message) : base(message)
        {
        }

        public RoomExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
