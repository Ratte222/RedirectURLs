using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ShortLink { get; set; }
        public string LongLink { get; set; }
        [NotMapped]
        public string BuildShortLink
        {
            get
            {
                return $"https://localhost:44330/re?sL={ShortLink}";
            }
        }
    }
}
