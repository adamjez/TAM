namespace OnRadio.BL.Models
{
    public class StreamModel
    {
        public string StreamUrl { get; set; }
        public bool IsActive { get; set; }
        public int Listeners { get; set; }
        public RadioModel Radio { get; set; }
    }
}