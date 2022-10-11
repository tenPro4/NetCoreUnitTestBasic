using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTest.Database
{
    public class IdentityDataContext : IdentityDbContext<AppUser>
    {

        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
