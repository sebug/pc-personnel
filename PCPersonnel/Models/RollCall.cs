using System;
using System.Collections.Generic;

namespace PCPersonnel.Models
{
    public class RollCall
    {
        public DateTime Date { get; set; }
        public string Entry { get; set; }
        public List<RollCallPerson> Presences { get; set; }
    }
}
