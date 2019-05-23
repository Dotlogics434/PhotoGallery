using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PixageStudioWeb.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Name { get; set; }
        [NotMapped]
        public IEnumerable<MvcControllerInfo> SelectedControllers { get; set; }
    }
}
