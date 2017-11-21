using GraphQL.Types;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web.Schema
{
    public class SampleSchema : GraphQL.Types.Schema
    {
        public SampleSchema(ObjectGraphType query)
        {
            Query = query;
        }
    }
}