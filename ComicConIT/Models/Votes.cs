using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicConIT.Models
{
    public partial class Votes
    {
        public Comics ID { get; set; }
        public List<String> UserName;
    }
}
