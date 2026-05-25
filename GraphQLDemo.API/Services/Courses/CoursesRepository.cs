using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Courses
{
    public class CoursesRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public CoursesRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<CourseDTO> Create(CourseDTO course)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<CourseDTO> Update(CourseDTO course)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            CourseDTO course = new CourseDTO()
            { 
                Id = id 
            };
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Courses.Remove(course);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
