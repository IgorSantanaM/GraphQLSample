using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Middlewares.UseUser;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;
        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        [Authorize]
        [UseUser]
        public async Task<CourseDTO> CreateCourse(CourseInputType courseInput,
            [Service] ITopicEventSender topicEventSender,
            [GlobalState] User user)
        {
            var userId = user.Id;
            var email = user.Email;
            var username = user.Username;
            CourseDTO course = new()
            {
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId,
                CreatorId = userId!
            };
            var courseReturn = await _coursesRepository.Create(course);

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return courseReturn;
        }

        [Authorize]
        [UseUser]

        public async Task<CourseDTO> UpdateCourse(Guid id, CourseInputType courseInput,
            [Service] ITopicEventSender topicEventSender, [GlobalState] User user)
        {
            var userId = user.Id;

            var currentCourse = await _coursesRepository.GetById(id);
            if (currentCourse is null)
                throw new GraphQLException("Course not found");

            if (currentCourse.CreatorId != userId)
                throw new GraphQLException("You do not have permission to update this course");

            currentCourse.Name = courseInput.Name;
            currentCourse.Subject = courseInput.Subject;
            currentCourse.InstructorId = courseInput.InstructorId;

            var returnCourse = await _coursesRepository.Update(currentCourse);

            //string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            //await topicEventSender.SendAsync(updateCourseTopic, course);

            return returnCourse;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<bool> DeleteCourse(Guid id)
            => await _coursesRepository.Delete(id);
    }
}
