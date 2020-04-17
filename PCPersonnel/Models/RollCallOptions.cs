using System;
using System.Collections.Generic;

namespace PCPersonnel.Models
{
    public class RollCallOptions
    {
        public DateTime Date { get; set; }
        public List<RollCallOption> Options { get; set; }
    }
}
