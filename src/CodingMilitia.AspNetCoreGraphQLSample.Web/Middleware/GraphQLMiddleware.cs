using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodingMilitia.AspNetCoreGraphQLSample.Web.Schema;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CodingMilitia.AspNetCoreGraphQLSample.Web.Middleware
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;

        public GraphQLMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<GraphQLMiddleware> logger, SampleSchema schema)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (ShouldRespondToRequest(context.Request))
            {
                var executionResult = await ExecuteAsync(context.Request, logger, schema);
                await WriteResponseAsync(context.Response, executionResult, logger);
                return;
            }

            await _next.Invoke(context);
        }

        private static bool ShouldRespondToRequest(HttpRequest request)
        {
            bool a = string.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase);
            bool b = request.Path.Equals("/graphql");
            return a && b;
        }

        private async Task<ExecutionResult> ExecuteAsync(HttpRequest request, ILogger<GraphQLMiddleware> logger, SampleSchema schema)
        {
            string requestBodyText;
            using (var streamReader = new StreamReader(request.Body))
            {
                requestBodyText = await streamReader.ReadToEndAsync();
            }

            dynamic graphqlRequest = JObject.Parse(requestBodyText);
            logger.LogInformation(requestBodyText);

            return await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = graphqlRequest.query;
            });
        }

        private Task WriteResponseAsync(HttpResponse response, ExecutionResult executionResult, ILogger<GraphQLMiddleware> logger)
        {
            response.ContentType = "application/json";
            response.StatusCode = (executionResult.Errors?.Count ?? 0) == 0 ? 200 : 400;
            var graphqlResponse = new DocumentWriter().Write(executionResult);
            if (executionResult.Errors != null)
            {
                foreach (var error in executionResult.Errors)
                {
                    logger.LogError(error.InnerException, "Unexpected error.");
                }
            }
            return response.WriteAsync(graphqlResponse);
        }
    }
}