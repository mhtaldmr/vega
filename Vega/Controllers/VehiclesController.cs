﻿using Microsoft.AspNetCore.Mvc;
using Vega.Models;
using System;
using System.Threading.Tasks;
using Vega.Controllers.Resources;
using AutoMapper;
using Vega.Persistance;


namespace Vega.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly VegaDbContext context;

        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleResource vehicleResource)
        {
            //if the user not send any data, we will return bad request
            //if (vehicleResource is null)
            //    return BadRequest("No data entered!");

            //if the user not send any data, we will return bad request : ANOTHER WAY with ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleResource vehicleResource)
        {
            //if the user not send any data, we will return bad request : ANOTHER WAY with ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var vehicle = await context.Vehicles.FindAsync(id);
            mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound();

            context.Remove(vehicle);
            await context.SaveChangesAsync();


            return Ok(id);
        }

    }
}
