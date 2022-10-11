using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using XTest.Database;
using XTesting.Services;

namespace XTest.UI.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public VotingPollFactory.Request Form { get; set; }

        public List<VotingPollVM> VotingPolls { get; set; }
        public void OnGet([FromServices] AppDbContext ctx)
        {
            VotingPolls = ctx.VotingPolls
               .Select(x => new VotingPollVM
               {
                   Id = EF.Property<int>(x, "Id"),
                   Title = x.Title
               })
               .ToList();
        }

        public IActionResult OnPost(
            [FromServices] IVotingPollFactory factory, 
            [FromServices]IVotingSystemPersistance persistance)
        {
            var poll = factory.Create(Form);

            persistance.SaveVotingPoll(poll);
            return RedirectToPage("/Index");
        }

        public class VotingPollVM
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
