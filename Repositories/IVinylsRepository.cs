using System;
using System.Collections.Generic;
using VinylCollection.Entities;

namespace VinylCollection.Repositories
{
    public interface IVinylsRepository
    {
        Vinyl GetVinyl(Guid id);
        IEnumerable<Vinyl> GetVinyls();
        void CreateVinyl(Vinyl vinyl);
        void UpdateVinyl(Vinyl vinyl);
        void DeleteVinyl(Guid id);
    }
}