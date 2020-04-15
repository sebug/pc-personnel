using System;
using System.Collections.Generic;

namespace PCPersonnel.Models
{
    public class StatsByDate
    {
        public string FormattedDate { get; set; }
        public int PresentCount { get; set; }
        public int RestCount { get; set; }
        public Dictionary<string, int> MissionCount { get; set; }
    }
}
