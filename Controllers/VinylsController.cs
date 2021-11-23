using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VinylCollection.Entities;
using VinylCollection.Repositories;

namespace VinylCollection.Controllers
{
    [ApiController]
    [Route("vinyls")]
    public class VinylsController : ControllerBase
    {
        private readonly IVinylsRepository _repository;

        public VinylsController(IVinylsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Vinyl> GetVinyls()
        {
            var vinyls = _repository.GetVinyls().Select(vinyl => vinyl);
            return vinyls;
        }

        [HttpGet("{id}")]
        public ActionResult<Vinyl> GetVinyl(Guid id)
        {
            var vinyl = _repository.GetVinyl(id);
            if (vinyl is null)
            {
                return NotFound();
            }
            return Ok(vinyl);
        }

        [HttpPost]
        public ActionResult<Vinyl> CreateVinyl(Vinyl v)
        {
            Vinyl vinyl = new()
            {
                Id = Guid.NewGuid(),
                Title = v.Title,
                Artist = v.Artist
            };
            _repository.CreateVinyl(vinyl);
            return CreatedAtAction(nameof(GetVinyl), new { id = vinyl.Id }, vinyl);
        }
        
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, Vinyl v)
        {
            var existingVinyl = _repository.GetVinyl(id);

            if (existingVinyl is null)
            {
                return NotFound();
            }
            
            existingVinyl.Artist = v.Artist;
            existingVinyl.Title = v.Title;

            _repository.UpdateVinyl(existingVinyl);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingVinyl = _repository.GetVinyl(id);

            if (existingVinyl is null)
            {
                return NotFound();
            }
            _repository.DeleteVinyl(id);
            return NoContent();
        }
    }
}