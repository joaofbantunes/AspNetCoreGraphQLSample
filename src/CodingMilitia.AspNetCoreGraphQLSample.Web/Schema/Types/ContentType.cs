using CodingMilitia.AspNetCoreGraphQLSample.Data.Model;
using GraphQL.Types;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web.Schema.Types
{
    public class ContentType : ObjectGraphType<Content>
    {
        public ContentType()
        {
            Name = "Content";

            Field(x => x.Id).Description("id");
            Field(x => x.Description).Description("description");
            Field<BoxType>("box", resolve: context => context.Source.Box);
        }
    }
}