using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
            var cel = _context.CelestialObjects.FirstOrDefault(n => n.Name == name);
            if (cel == null) return NotFound();
            cel.Satellites = _context.CelestialObjects.Where(n => n.OrbitedObjectId == cel.Id).ToList();

            return Ok(cel);
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
    }
}
