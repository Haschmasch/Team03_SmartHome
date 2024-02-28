namespace MainUnit.Models.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException()
        {
        }

        public RoomNotFoundException(string? message) : base(message)
        {
        }

        public RoomNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
