using System;

namespace VinylCollection.Entities
{
    public class Vinyl
    {
        public Guid Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
    }
}