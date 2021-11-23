using System;
using System.Collections.Generic;
using System.Linq;
using VinylCollection.Entities;


namespace VinylCollection.Repositories
{
        public class VinylsRepository : IVinylsRepository
        {
        private readonly List<Vinyl> _collection = new()
        {
            new Vinyl { Id = Guid.NewGuid(), Artist = "Bob Dylan", Title = "New Morning" },
            new Vinyl { Id = Guid.NewGuid(), Artist = "Leonard Cohen", Title = "Ten New Songs" },
            new Vinyl { Id = Guid.NewGuid(), Artist = "Flamingokvintetten", Title = "Flamingokvintetten 12" }
        };
        public IEnumerable<Vinyl> GetVinyls()
        {
            return _collection;
        }
        public Vinyl GetVinyl(Guid id)
        {
            var vinyl = _collection.Where(vinyl => vinyl.Id == id);
            return vinyl.SingleOrDefault();
        }
        public void CreateVinyl(Vinyl vinyl)
        {
            _collection.Add(vinyl);
        }
        public void UpdateVinyl(Vinyl vinyl)
        {
            var index = _collection.FindIndex(exVinyl => exVinyl.Id == vinyl.Id);
            _collection[index] = vinyl;
        }
        public void DeleteVinyl(Guid id)
        {
            var index = _collection.FindIndex(exVinyl => exVinyl.Id == id);
            _collection.RemoveAt(index);
        }
    }
}