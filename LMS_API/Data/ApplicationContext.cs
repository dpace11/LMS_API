using LMS_API.Models;
using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options)
        {
            
        }

        public DbSet<Books> Books { get; set; }

        public DbSet<Students> Students { get; set; }
    }
}
