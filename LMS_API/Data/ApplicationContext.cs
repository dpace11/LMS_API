using LMS_API.Models.Author;
using LMS_API.Models.BookIssueReturn;
using LMS_API.Models.Books;
using LMS_API.Models.Membership;
using LMS_API.Models.Publication;
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

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publication> Publications { get; set; }

        public DbSet<BookIssueReturn> BookIssueReturns { get; set; }
    }
}
