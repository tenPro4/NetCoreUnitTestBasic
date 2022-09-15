using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Database.Models
{
    public class Course : BaseEntity
    {

        public string Url { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Tags { get; set; }

        public byte[] Image { get; set; }
    }
}
