namespace MainUnit.Models.Exceptions
{
    public class ThermostatExistsException : Exception
    {
        public ThermostatExistsException()
        {
        }

        public ThermostatExistsException(string? message) : base(message)
        {
        }

        public ThermostatExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
