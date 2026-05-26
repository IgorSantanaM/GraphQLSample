using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
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
        public async Task<CourseDTO> CreateCourse(CourseInputType courseInput,
            [Service] ITopicEventSender topicEventSender,
            ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            var email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            var username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            CourseDTO course = new()
            {
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };
            var courseReturn = await _coursesRepository.Create(course);

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return courseReturn;
        }

        [Authorize]
        public async Task<CourseDTO> UpdateCourse(Guid id, CourseInputType courseInput,
            [Service] ITopicEventSender topicEventSender)
        {
            CourseDTO course = new()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };

            var returnCourse = await _coursesRepository.Update(course);

            //string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            //await topicEventSender.SendAsync(updateCourseTopic, course);

            return returnCourse;
        }

        [Authorize]
        public async Task<bool> DeleteCourse(Guid id)
            => await _coursesRepository.Delete(id);
    }
}
