namespace OnRadio.BL.Models
{
    public class StreamModel
    {
        public enum StreamQuality
        {
            High,
            Low
        }

        public StreamQuality Quality { get; set; }
        public string StreamUrl { get; set; }
        public bool IsActive { get; set; }
        public int Listeners { get; set; }
        public RadioModel Radio { get; set; }
    }
}