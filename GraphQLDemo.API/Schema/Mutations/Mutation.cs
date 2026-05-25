using GraphQLDemo.API.Schema.Queries;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        public CourseResult CreateCourse(CourseInputType courseInput)
        {

            CourseResult course = new()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };

            _courses.Add(course);

            return course;
        }

        public CourseResult UpdateCourse(Guid id, CourseInputType courseInput)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);

            if (course is null)
            {
                throw new GraphQLException("Course not found");
            }

            course.Name = courseInput.Name;
            course.Subject = courseInput.Subject;
            course.InstructorId = courseInput.InstructorId;

            return course;
        }

        public bool DeleteCourse(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) >= 1; 
        }
    }
}
