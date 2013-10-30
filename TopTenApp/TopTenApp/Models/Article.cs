using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTenApp.Models
{
    public class Article
    {
        public string ObjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Ranglist> Ranglist { get; set; }
        public string Image { get; set; }
    }
}
