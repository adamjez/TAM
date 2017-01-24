namespace OnRadio.App.Exceptions
{
    public class StreamsNotFoundException : AppException
    {
        public StreamsNotFoundException()
            : base("Streams Not Found", "Radio zrovna nevysílá", "Ups")
        {
        }
    }
}