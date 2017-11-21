using System;
using System.Collections.Generic;
using System.Linq;
using CodingMilitia.AspNetCoreGraphQLSample.Data;
using CodingMilitia.AspNetCoreGraphQLSample.Data.Model;
using CodingMilitia.AspNetCoreGraphQLSample.Web.Schema.Types;
using GraphQL.Language.AST;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web.Schema
{
    public class EFBasedSampleQuery : ObjectGraphType
    {
        private readonly SampleContext _db;
        public EFBasedSampleQuery(SampleContext db)
        {
            _db = db;
            Name = "Query";

            FieldAsync<BoxType>("box",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the box" }
                    ),
                    resolve: async context =>
                    {
                        var id = int.Parse(context.GetArgument<string>("id"));
                        IQueryable<Box> dbQuery = _db.Boxes;
                        if (AreBoxContentsRequestedForSingle(context))
                        {
                            dbQuery = dbQuery.Include(b => b.Contents);
                        }
                        return await dbQuery.SingleOrDefaultAsync(b => b.Id == id, context.CancellationToken);
                    });

            FieldAsync<ListGraphType<BoxType>>("boxes",
                    resolve: async context =>
                    {
                        IQueryable<Box> dbQuery = _db.Boxes;
                        if (AreBoxContentsRequestedForCollection(context))
                        {
                            dbQuery = dbQuery.Include(b => b.Contents);
                        }
                        return await dbQuery.ToListAsync(context.CancellationToken);
                    });

            // FieldAsync<ContentType>("content",
            //     arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
            //     resolve: async context =>
            //     {
            //         var id = int.Parse(context.GetArgument<string>("id"));
            //         return await _db.Contents.SingleOrDefaultAsync(c => c.Id == id, context.CancellationToken);
            //     });
        }

        private bool AreBoxContentsRequestedForSingle(ResolveFieldContext<object> context)
        {
            //not a fan of this approach, but couldn't find a better one yet...
            return IsFieldRequested(FindField(context.Document, "box").SelectionSet.Selections, "contents");
        }

        private bool AreBoxContentsRequestedForCollection(ResolveFieldContext<object> context)
        {
            //not a fan of this approach, but couldn't find a better one yet...
            return IsFieldRequested(FindField(context.Document, "boxes").SelectionSet.Selections, "contents");
        }

        private static Field FindField(INode node, string name)
        {
            if (node == null)
            {
                return null;
            }
            var field = (node as Field);
            if (field?.Name == name)
            {
                return field;
            }
            foreach (var childNode in node.Children)
            {
                field = FindField(childNode, name);
                if (field != null)
                {
                    return field;
                }
            }

            return null;
        }

        private static bool IsFieldRequested(IEnumerable<INode> nodes, string name)
        {
            if (nodes == null)
            {
                return false;
            }
            return nodes.Any(n => (n as Field)?.Name == name);
        }
    }
}