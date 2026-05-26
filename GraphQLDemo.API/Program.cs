using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()
    .AddFiltering();

string connectionString = builder.Configuration.GetConnectionString("default")!;

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionString: connectionString));

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<SchoolDbContext>();
    dbContext.Database.Migrate();
}

app.UseWebSockets();
app.MapGet("/", () => "Hello World!");

app.MapGraphQL();

app.Run();