using Bogus;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Filters;
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
        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            var coursesDto = await _courseRepository.GetAll();

            return coursesDto.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
            });
        }
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseFiltering(typeof(CourseFilterType))]
        public IQueryable<CourseType> GetPaginatedCourses([Service] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
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
            };
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string GetHello() => "Hello from GraphQL!";
    }
}
