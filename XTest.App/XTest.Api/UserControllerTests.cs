using AutoMapper;
using Blog.Turnmeup.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.App.Controllers;
using XTest.Database.Models;
using XTesting.Services.Infrastructure.ErrorHandler;
using XTesting.Services.Models;
using XTesting.Services.Repositories;
using XTesting.Services.Services;

namespace XTest.Api
{
    public class UserControllerTests : IClassFixture<VotingFixture>
    {
        private readonly VotingFixture _fixture;

        private IUsersRepository Repository { get; }
        private IUsersService Service { get; }
        private UsersController Controller { get; }

        public UserControllerTests(VotingFixture fixture)
        {
            _fixture = fixture;

            var users = new List<UserModel>
            {
                new UserModel
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users).Returns(users);

            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<UserModel>()))
          .ReturnsAsync(IdentityResult.Success);

            Repository = new UsersRepository(fakeUserManager.Object);

            var mapper = (IMapper)fixture.Services.GetService(typeof(IMapper));
            var errorHandler = (IErrorHandler)fixture.Services.GetService(typeof(IErrorHandler));
            var passwordhasher = new Mock<IPasswordHasher<AppUser>>();

            var uservalidator = new Mock<IUserValidator<AppUser>>();
            uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
             .ReturnsAsync(IdentityResult.Success);
            var passwordvalidator = new Mock<IPasswordValidator<AppUser>>();
            passwordvalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            var signInManager = new Mock<FakeSignInManager>();

            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);


            //SERVICES CONFIGURATIONS
            Service = new UsersService(Repository, mapper, uservalidator.Object, passwordvalidator.Object, passwordhasher.Object, signInManager.Object);
            Controller = new UsersController(Service, errorHandler);
        }

        [Theory]
        [InlineData("test@test.it", "Ciao.Ciao", "Test_user")]
        public async Task Insert(string email, string password, string name)
        {
            //Arrange
            var testUser = new CreateRequestModel
            {
                Email = email,
                Name = name,
                Password = password
            };
            //Act
            var createdUser = await Controller.Create(testUser);
            //Assert
            Assert.Equal(email, createdUser.Email);
        }

        [Fact]
        public void Get()
        {
            //Act
            var result = Controller.Get();
            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("test@test.it")]
        public void GetByEmail(string email)
        {
            //Act
            var result = Controller.Get(email);
            // Assert
            Assert.Equal(result.Email, email);
        }



        [Theory]
        [InlineData("test@test.it", "password", "Test")]
        public async Task Update(string email, string password, string name)
        {
            //Arrange
            var testUser = new UpdateRequestModel
            {
                Email = email,
                Password = password
            };

            //Act
            var updated = await Controller.Edit(testUser);
            // Assert
            Assert.Equal(email, updated.Email);
        }

        [Theory]
        [InlineData("test@test.it", "Ciao.Ciao")]
        public async Task Delete(string email, string password)
        {
            //Arrange
            var testUser = new DeleteRequestModel
            {
                Email = email,
                Password = password
            };
            //Act
            var deleted = await Controller.Delete(testUser);
            //Assert
            Assert.Equal(email, deleted.Email);
        }

        [Theory]
        [InlineData("test@test.it", "Ciao.Ciao")]
        public async Task TokenAsync(string email, string password)
        {
            //Arrange
            var testUser = new LoginRequestModel
            {
                Email = email,
                Password = password
            };

            //Act
            var updated = await Controller.Token(testUser);
            // Assert
            Assert.Equal("Test", updated.Principal.Identity.Name);
        }
    }
}
