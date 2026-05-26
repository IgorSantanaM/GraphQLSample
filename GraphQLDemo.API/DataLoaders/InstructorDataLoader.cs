using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly InstructorsRepository _instructorsRepository;

        public InstructorDataLoader(IBatchScheduler batchScheduler, 
            DataLoaderOptions options, 
            InstructorsRepository instructorsRepository)
            : base(batchScheduler, options)
        {
            _instructorsRepository = instructorsRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var instructors = await _instructorsRepository.GetManyByIds(keys);

            return instructors.ToDictionary(i => i.Id);
        }
    }
}
