# ASP.NET Core GraphQL sample

Just a quick sample to try GraphQL with ASP.NET Core.
Using EF Core for data access and Postgres as data source.

## Testing

Running a docker container with postgres for tests
docker run -p 5432:5432 --restart unless-stopped --name postgres -e POSTGRES_USER=user -e POSTGRES_PASSWORD=pass -d postgres