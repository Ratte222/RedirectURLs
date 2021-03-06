using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
