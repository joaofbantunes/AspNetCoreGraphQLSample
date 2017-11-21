using CodingMilitia.AspNetCoreGraphQLSample.Data.Model;
using GraphQL.Types;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web.Schema.Types
{
    public class BoxType : ObjectGraphType<Box>
    {
        public BoxType()
        {
            Name = "Box";

            Field(x => x.Id).Description("id");
            Field(x => x.Label).Description("label");
            Field(x => x.Brand).Description("brand");
            Field(x => x.Width).Description("width");
            Field(x => x.Height).Description("height");
            Field(x => x.Depth).Description("depth");
            Field<ListGraphType<ContentType>>("contents", resolve: context => context.Source.Contents);
        }
    }
}