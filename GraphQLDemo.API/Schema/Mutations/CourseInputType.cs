using GraphQLDemo.API.Schema.Queries;

namespace GraphQLDemo.API.Schema.Mutations
{
    public record CourseInputType(string Name, Subject Subject, Guid InstructorId);
}
