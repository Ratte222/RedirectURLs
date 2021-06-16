using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Models
{
    public class UserLinks
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Link> Links { get; set; }
    }
}
