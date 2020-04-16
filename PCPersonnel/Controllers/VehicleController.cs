using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PCPersonnel.Entities;
using PCPersonnel.Repositories;

namespace PCPersonnel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleController(IVehicleRepository vehicleRepository)
        {
            this._vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public IEnumerable<Vehicle> Get()
        {
            var vehicles = this._vehicleRepository.GetAll();

            return vehicles;
        }
    }
}
