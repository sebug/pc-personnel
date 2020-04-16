using System;
using System.Collections.Generic;
using PCPersonnel.Entities;
using System.Linq;

namespace PCPersonnel.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleContext _vehicleContext;
        public VehicleRepository(VehicleContext vehicleContext)
        {
            this._vehicleContext = vehicleContext;
        }

        public List<Vehicle> GetAll()
        {
            return this._vehicleContext.Vehicle.ToList();
        }
    }
}
