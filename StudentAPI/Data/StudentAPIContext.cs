using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;

namespace StudentAPI.Data
{
    public class StudentAPIContext : DbContext
    {
        public StudentAPIContext (DbContextOptions<StudentAPIContext> options)
            : base(options)
        {
        }

        public DbSet<StudentAPI.Models.Student> Student { get; set; } = default!;
        public DbSet<StudentAPI.Models.Enrollment> Enrollment { get; set; } = default!;
    }
}
