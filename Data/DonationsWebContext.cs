using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DonationsWeb.Models;

namespace DonationsWeb.Data
{
    public class DonationsWebContext : DbContext
    {
        public DonationsWebContext (DbContextOptions<DonationsWebContext> options)
            : base(options)
        {
        }

        public DbSet<DonationsWeb.Models.User> User { get; set; } = default!;
    }
}
