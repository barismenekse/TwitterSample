using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Context
{
    public class IdDbContext : IdentityDbContext<IdentityUser>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options):base(options)
        {

        }
    }
}
