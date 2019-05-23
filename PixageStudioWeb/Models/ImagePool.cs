using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PixageStudioWeb.Models
{
    public class ImagePool
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string AltName { get; set; }
        [DisplayName("Path")]
        public string ImagePath { get; set; }
        [DisplayName("Category")]
        public string Genre { get; set; }
    }
}
