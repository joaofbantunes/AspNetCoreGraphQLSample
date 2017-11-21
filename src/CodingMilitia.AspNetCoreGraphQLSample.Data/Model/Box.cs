using System.Collections.Generic;

namespace CodingMilitia.AspNetCoreGraphQLSample.Data.Model
{
    public class Box
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Brand { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}