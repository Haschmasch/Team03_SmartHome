namespace MainUnit.Models.Exceptions
{
    public class ThermostatNotFoundException : Exception
    {
        public ThermostatNotFoundException()
        {
        }

        public ThermostatNotFoundException(string? message) : base(message)
        {
        }

        public ThermostatNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
