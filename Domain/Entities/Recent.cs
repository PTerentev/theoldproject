using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Recent
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
        public string FullText { get; set; }
        public byte[] LinkSmallImg { get; set; }
        public byte[] LinkLargeImg { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
    }
}
