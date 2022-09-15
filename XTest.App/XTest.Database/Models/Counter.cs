﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Database.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Count { get; set; }
        public double? Percent { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
