using FirebaseAdmin.Auth;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Services.Instructors;
using System.Reflection.Metadata.Ecma335;

namespace GraphQLDemo.API.Schema.Queries
{
    public enum Subject
    {
        Mathematics,
        Science,
        History
    }

    public class CourseType : ISearchResultType
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
        [IsProjected(true)]
        public string? CreatorId { get; set; }
        public async Task<UserType?> Creator([Service] UserDataLoader userDataLoader)
        {
            if (CreatorId is null)
                return null!;

            return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);
        }
    }
}
