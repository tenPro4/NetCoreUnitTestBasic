using AutoMapper;
using Microsoft.AspNetCore.Identity;
using XTest.Database.Models;
using XTesting.Services.Models;

namespace XTest.App.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Course, CourseResponseModel>();
            CreateMap<CourseResponseModel, Course>();

            CreateMap<UserModel, AppUser>();
            CreateMap<AppUser, UserModel>();

            CreateMap<UserManager<UserModel>, UserManager<AppUser>>();
            CreateMap<UserManager<AppUser>, UserManager<UserModel>>();
        }
    }
}
