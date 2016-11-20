namespace OnRadio.App.Exceptions
{
    public class RadioNotFoundException : AppException
    {
        public RadioNotFoundException() 
            : base("Radio Not Found", "Radio zrovna nevysílá", "Ups")
        {
        }
    }
}