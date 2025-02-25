using Microsoft.EntityFrameworkCore;
using PranayChauhanProjectAPI.Models.Domain;


namespace PranayChauhanProjectAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        // Instance of DB Context Class  Work with Domian Classfor doing Relations to the Databases
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<BlogsPost> BlogsPosts { get; set; }    

        public DbSet<Category> Categories { get; set; }     
        public DbSet<BlogImage> BlogImage { get; set; }     
    }
}
