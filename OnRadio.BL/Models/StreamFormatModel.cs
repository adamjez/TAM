using System.Collections.Generic;

namespace OnRadio.BL.Models
{
    public class StreamFormatModel
    {
        public string Format { get; set; }
        public List<int> Bitrates { get; set; }
    }
}