using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCPersonnel.Entities;
using PCPersonnel.Repositories;

namespace PCPersonnel.Pages
{
    public class VehiclesModel : PageModel
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehiclesModel(IVehicleRepository vehicleRepository)
        {
            this._vehicleRepository = vehicleRepository;
        }

        public List<Vehicle> Vehicles { get; set; }

        public void OnGet()
        {
            this.Vehicles = this._vehicleRepository.GetAll()
                .OrderBy(vhc => vhc.Code).ToList();
        }
    }
}
