using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NARAOURCEISG.Models;

    public class NARAOUCREISGDBContext : DbContext
    {
        public NARAOUCREISGDBContext (DbContextOptions<NARAOUCREISGDBContext> options)
            : base(options)
        {
        }

        public DbSet<NARAOURCEISG.Models.User> User { get; set; } = default!;
    }
