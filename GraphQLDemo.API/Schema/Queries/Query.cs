using Bogus;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Filters;
using GraphQLDemo.API.Schema.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        private readonly CoursesRepository _courseRepository;
        public Query(CoursesRepository coursesRepository)
        {
            _courseRepository = coursesRepository;
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string GetHello() => "Hello from GraphQL!";
    }
}
