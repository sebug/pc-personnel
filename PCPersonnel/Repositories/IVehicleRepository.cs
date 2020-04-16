using System;
using System.Collections.Generic;
using PCPersonnel.Entities;

namespace PCPersonnel.Repositories
{
    public interface IVehicleRepository
    {
        List<Vehicle> GetAll();
    }
}
