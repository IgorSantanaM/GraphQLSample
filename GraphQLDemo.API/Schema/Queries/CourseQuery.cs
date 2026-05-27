using GraphQLDemo.API.Schema.Filters;
using GraphQLDemo.API.Schema.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class CourseQuery
    {
        private readonly CoursesRepository _courseRepository;
        public CourseQuery(CoursesRepository coursesRepository)
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

        public async Task<IEnumerable<ISearchResultType>> Search(string term, [Service] SchoolDbContext context)
        {
            var courses = await context.Courses
                .Where(c => c.Name.Contains(term))
                .Select(c => new CourseType()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId,
                    CreatorId = c.CreatorId
                }).ToListAsync();

            var instructors = await context.Instructors
                .Where(i => i.FirstName!.Contains(term) || i.LastName!.Contains(term))
                .Select(i => new InstructorType()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Salary = i.Salary,
                }).ToListAsync();

            return new List<ISearchResultType>().Concat(courses).Concat(instructors);
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
    }
}
