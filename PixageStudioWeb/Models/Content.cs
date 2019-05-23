using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PixageStudioWeb.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public string Category { get; set; }
        public string FontName { get; set; }
        public string FontSize { get; set; }
        [ForeignKey("ImageId")]
        public int ImageId { get; set; }
        public virtual ImagePool ImagePool { get; set; }

    }
}
