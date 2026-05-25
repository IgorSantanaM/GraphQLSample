using Bogus;

namespace GraphQLDemo.API.Schema
{
    public class Query
    {
        private readonly Faker<StudentType> _studentFaker;
        private readonly Faker<InstructorType> _instructorFake;
        private readonly Faker<CourseType> _courseFaker;
        public Query()
        {
            _studentFaker = new Faker<StudentType>()
               .RuleFor(c => c.Id, f => Guid.NewGuid())
               .RuleFor(c => c.FirstName, f => f.Name.FirstName())
               .RuleFor(c => c.LastName, f => f.Name.LastName())
               .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));

            _instructorFake = new Faker<InstructorType>()
               .RuleFor(c => c.Id, f => Guid.NewGuid())
               .RuleFor(c => c.FirstName, f => f.Name.FirstName())
               .RuleFor(c => c.LastName, f => f.Name.LastName())
               .RuleFor(c => c.Salary, f => f.Random.Double(0, 100000));

            _courseFaker = new Faker<CourseType>()
               .RuleFor(c => c.Id, f => Guid.NewGuid())
               .RuleFor(c => c.Name, f => f.Name.JobTitle())
               .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
               .RuleFor(c => c.Instructor, f => _instructorFake.Generate())
               .RuleFor(c => c.Students, f => _studentFaker.Generate(3));
        }

        public IEnumerable<CourseType> GetCourses()
            => _courseFaker.Generate(5);

        public async Task<CourseType> GetCourseById(Guid id)
        {

            await Task.Delay(TimeSpan.FromSeconds(1));
            CourseType course = _courseFaker.Generate();

            course.Id = id;

            return course;
        }


        [GraphQLDeprecated("This query is deprecated.")]
        public string GetHello() => "Hello from GraphQL!";
    }
}
