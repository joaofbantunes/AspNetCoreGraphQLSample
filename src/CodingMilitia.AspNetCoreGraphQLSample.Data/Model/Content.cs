namespace CodingMilitia.AspNetCoreGraphQLSample.Data.Model
{
    public class Content
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Box Box { get; set; }
    }
}