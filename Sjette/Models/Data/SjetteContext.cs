using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sjette.Models;


namespace Sjette.Models.Data
{
    public class SjetteContext : DbContext
    {

        // Initialize the Sjette constructor.
        public SjetteContext(DbContextOptions<SjetteContext> options) : base(options) { }

        // Initialize the Sjette-User constructor.
        public DbSet<Sjette.Models.Users> Users { get; set; }

        // Initialize the Sjette-Activities constructor.
        public DbSet<Sjette.Models.Activities> Activities{ get; set; }

        // Initialize the Sjette-Groups constructor.
        public DbSet<Sjette.Models.Groups> Groups { get; set; }

        // Initialize the Sjette-GroupMembership constructor.
        public DbSet<Sjette.Models.GroupMembership> GroupMembership { get; set; }


        // Initialize the Sjette-MutualUsers constructor.
        public DbSet<Sjette.Models.MutualUsers> MutualUsers { get; set; }
    }
}
