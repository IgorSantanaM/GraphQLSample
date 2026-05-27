using Bogus;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Filters;
using GraphQLDemo.API.Schema.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        private readonly CoursesRepository _courseRepository;
        public Query(CoursesRepository coursesRepository)
        {
            _courseRepository = coursesRepository;
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting(typeof(CourseSortType))]
        public IQueryable<CourseType> GetCourses([Service] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId
            });
        }

        public async Task<CourseType?> GetCourseById(Guid id)
        {
            var course = await _courseRepository.GetById(id);

            if (course == null)
                return null!;
            return new CourseType()
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId,
                CreatorId = course.CreatorId

            };
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string GetHello() => "Hello from GraphQL!";
    }
}
