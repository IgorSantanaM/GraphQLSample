using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.Schema.Queries
{
    public enum Subject
    {
        Mathematics,
        Science,
        History
    }

    public class CourseType
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Subject? Subject { get; set; }
        [IsProjected(true)]
        public Guid InstructorId { get; set; }
        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            var instructorDto = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);
            return new InstructorType()
            {
                Id = instructorDto.Id,
                FirstName = instructorDto.FirstName,
                LastName = instructorDto.LastName,
                Salary = instructorDto.Salary,
            };
        }

        public IEnumerable<StudentType>? Students { get; set; }
    }
}
