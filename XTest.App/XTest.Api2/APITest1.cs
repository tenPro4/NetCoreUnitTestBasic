using FluentAssertions;
using System.Net;
using XTest.Api2.Common;
using XTest.Api2.Fixtures;

namespace XTest.Api2
{
    public class APITest1 : BaseClassFixture
    {
        public APITest1(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetMapping()
        {
            //set jwt token for auth
            SetupAdminClaimsViaHeaders();

            var response = await Client.GetAsync("/api/home");

            //assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //[Fact]
        //public async Task ReturnSuccessWithAdminRole()
        //{
        //    SetupAdminClaimsViaHeaders();

        //    foreach (var route in RoutesConstants.GetConfigureRoutes())
        //    {
        //        // Act
        //        var response = await Client.GetAsync($"/Configuration/{route}");

        //        // Assert
        //        response.EnsureSuccessStatusCode();
        //        response.StatusCode.Should().Be(HttpStatusCode.OK);
        //    }
        //}

        //check whether got redirect to login page if not authorize
        //[Fact]
        //public async Task GetApiResourcesWithoutPermissions()
        //{
        //    Client.DefaultRequestHeaders.Clear();

        //    var response = await Client.GetAsync("api/apiresources");

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        //    //The redirect to login
        //    response.Headers.Location.ToString().Should().Contain(AuthenticationConsts.AccountLoginPage);
        //}
    }
}