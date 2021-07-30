using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGallery_WPF.Models
{
    public class Image
    {
        public string Name { get; set; }
        public string Source { get; set; }

        public Image() { }
        public Image(string name, string source)
        {
            Name = name;
            Source = source;
        }
    }
}
