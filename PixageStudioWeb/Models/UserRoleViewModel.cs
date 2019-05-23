using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PixageStudioWeb.Models
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public string UserId { get; internal set; }
        public string UserName { get; internal set; }
        [NotMapped]
        public IEnumerable<string> Roles { get; internal set; }
    }
}
