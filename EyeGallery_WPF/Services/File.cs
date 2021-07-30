using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGallery_WPF.Services
{
    public static class File
    {
        public static List<Models.Image> ReadJSON(string filename)
        {
            var serializer = new JsonSerializer();
            var fs = new FileStream(filename, FileMode.OpenOrCreate);
            using (var sr = new StreamReader(fs))
            using (var jr = new JsonTextReader(sr))
                return serializer.Deserialize<List<Models.Image>>(jr);
        }

        public static void WriteJSON(List<Models.Image> images, string filename)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(filename))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                serializer.Serialize(jw, images);
            }
        }
    }
}
