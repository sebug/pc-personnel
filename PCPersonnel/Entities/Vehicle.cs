using System;
using System.ComponentModel.DataAnnotations;

namespace PCPersonnel.Entities
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }
        public string Code { get; set; }
        public string Mission { get; set; }
        public string VehicleType { get; set; }
    }
}
