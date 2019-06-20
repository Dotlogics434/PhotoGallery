using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixageStudioWeb.Models
{
    public partial class MultiModel
    {
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<ImagePool> images { get; set; }
        public Login login { get; set; }
      
    }
}
