using System;
namespace PCPersonnel.Models
{
    public class PresenceEntry
    {
        public DateTime Date { get; set; }
        public bool Called { get; set; }
        public string Presence { get; set; }
    }
}
