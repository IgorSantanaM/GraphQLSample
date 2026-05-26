using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public InstructorsRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<InstructorDTO?> GetById(Guid instructorId)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .FirstOrDefaultAsync(c => c.Id == instructorId);
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            }
        }
    }
}
