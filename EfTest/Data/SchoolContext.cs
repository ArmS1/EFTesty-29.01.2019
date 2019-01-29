using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfTest.Models
{
    public class SchoolContext : DbContext
    {
        public DbSet<EfTest.Models.Student> Student { get; set; }
        public DbSet<EfTest.Models.Grade> Grade { get; set; }

        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

    }
}
