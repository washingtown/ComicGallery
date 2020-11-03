using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string DefaultDir { get; set; }
        public List<Comic> Comics { get; set; }
    }
}
