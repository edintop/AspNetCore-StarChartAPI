using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var cel = _context.CelestialObjects.FirstOrDefault(n => n.Id == id);
            if (cel == null) return NotFound();
            cel.Satellites = _context.CelestialObjects.Where(n => n.OrbitedObjectId == cel.Id).ToList();

            return Ok(cel);
        }


        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var cels = _context.CelestialObjects.Where(n => n.Name == name);
            if (!cels.Any()) return NotFound();
            foreach (var cel in cels)
            {
                cel.Satellites = _context.CelestialObjects.Where(n => n.OrbitedObjectId == cel.Id).ToList();
            }

            return Ok(cels);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            foreach (var cel in _context.CelestialObjects)
            {
                cel.Satellites = _context.CelestialObjects.Where(n => n.OrbitedObjectId == cel.Id).ToList();
            }
           
            return Ok(_context.CelestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var cel = _context.CelestialObjects.FirstOrDefault(n => n.Id == id);
            if (cel == null) return NotFound();
            cel.Name = celestialObject.Name;
            cel.OrbitalPeriod = celestialObject.OrbitalPeriod;
            cel.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(cel);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var cel = _context.CelestialObjects.FirstOrDefault(n => n.Id == id);
            if (cel == null) return NotFound();
            cel.Name = name;


            _context.CelestialObjects.Update(cel);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id, string name)
        {
            var cels = _context.CelestialObjects.Where(n => n.Id == id || n.OrbitedObjectId == id);
            if (!cels.Any()) return NotFound();


            _context.CelestialObjects.RemoveRange(cels);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
