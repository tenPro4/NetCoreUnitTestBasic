using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using XTest.Api.Infrastructure;
using XTest.TestUI.Fixtures;
using XTest.TestUI.Infrastructure;

namespace XTest.TestUI
{
    public class VotingTest:IClassFixture<VotingFixture>
    {
        private readonly VotingFixture _fixture;

        public VotingTest(VotingFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task OnGet()
        {
            var client = _fixture.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            var pollPage = await client.GetAsync("/poll/1");
            var pollHtml = await pollPage.Content.ReadAsStringAsync();

            var cookieToken = AntiForgeryUtils.ExtractCookieToken(pollPage.Headers);
            var formToken = AntiForgeryUtils.ExtractFormToken(pollHtml, "test_csrf_field");

            var request = new HttpRequestMessage(HttpMethod.Post, "/Poll/1");
            request.Content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("counterId", "1"),
                new KeyValuePair<string, string>("test_csrf_field", formToken)
            });
            request.Headers.Add("Cookie", $"test_csrf_cookie={cookieToken}");
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            DbContextUtils.ActionDatabase(_fixture.Services, ctx =>
            {
                var vote = ctx.Votes.Single();
                Assert.Equal("test@test.com", vote.UserId);
                Assert.Equal(1, vote.CounterId);

                var counter = ctx.Counters.Include(x => x.Votes).First(x => x.Id == 1);
                Assert.Single(counter.Votes);
            });
        }
    }
}