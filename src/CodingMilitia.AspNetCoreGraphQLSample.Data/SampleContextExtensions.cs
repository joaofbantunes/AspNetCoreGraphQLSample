using System;
using System.Collections.Generic;
using System.Linq;
using CodingMilitia.AspNetCoreGraphQLSample.Data.Model;

namespace CodingMilitia.AspNetCoreGraphQLSample.Data
{
    public static class SampleContextExtensions
    {
        public static void EnsureSeedData(this SampleContext ctx)
        {
            if (!ctx.Boxes.Any())
            {
                var r = new Random();
                for (var i = 0; i < 20; ++i)
                {
                    ctx.Boxes.Add(new Box
                    {
                        Label = "Box " + i,
                        Brand = i % 2 == 0 ? "IKEA" : "LIDL",
                        Width = 10 + r.Next(20),
                        Height = 10 + r.Next(20),
                        Depth = 10 + r.Next(20),
                        Contents = GetContents(i)
                    });

                    ctx.SaveChanges();
                }
            }
        }

        private static ICollection<Content> GetContents(int i)
        {
            var r = new Random(i);
            var result = new List<Content>();

            for (var j = 0; j < r.Next(20); ++j)
            {
                result.Add(
                    new Content
                    {
                        Description = $"Item {j} from box {i}"
                    }
                );
            }

            return result;
        }
    }
}