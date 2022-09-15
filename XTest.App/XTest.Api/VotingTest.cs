using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XTesting.Services;

namespace XTest.Api
{
    public class VotingTest:IClassFixture<VotingFixture>
    {
        private readonly VotingFixture _fixture;

        public VotingTest(VotingFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetPolls()
        {
            var client = _fixture.CreateClient();
            var result = await client.GetAsync("/api/polls");
            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPollById()
        {
            var client = _fixture
                .AuthenticatedInstance(new Claim("CustomRoleType","God"))
                .CreateClient(new()
                {
                    AllowAutoRedirect = false
                });

            var result = await client.GetAsync("/api/polls/1");
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task SavesFileToDisk()
        {
            var client = _fixture.CreateClient();

            MultipartFormDataContent form = new();

            form.Add(new StreamContent(_fixture.TestFile), "file", "base.png");
            var response = await client.PostAsync("/api/polls/upFile", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fileResponse = await client.GetAsync("/test_images/base.png");
            Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
        }

        [Fact]
        public void TestSplit()
        {
            string sentence = "the quick brown fox jumps over the lazy dog";

            // Split the string into individual words.
            string[] words = sentence.Split(' ');
            string reversed = words.Aggregate((workingSentence, next) =>
                                      workingSentence+next);

            Assert.Contains("fox",reversed);
        }
    }
}
