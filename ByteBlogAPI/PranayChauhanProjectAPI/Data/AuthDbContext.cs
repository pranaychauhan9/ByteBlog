using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PranayChauhanProjectAPI.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var readerRoleId =  Guid.Parse("2c902f6f-370f-4846-a43c-113c22073a85");
            var writerRoleId = Guid.Parse("57845a31-2c11-4f7e-a1e3-d5f0716bdebb");


            //Create Reader And Writer  Role
            var roles = new List<IdentityRole>
            {

                new IdentityRole
                {
                    Id = readerRoleId.ToString(),
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId.ToString(),
                },


                new IdentityRole
                {
                    Id = writerRoleId.ToString(),
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId.ToString(),
                }

            };



            //Seed Roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create An Admin User
            var adminUserId = Guid.Parse("be1988b3-891b-4a02-92e4-c830224d12ba");
            var admin = new IdentityUser()
            {
                Id = adminUserId.ToString(),
                UserName = "admin@pc.com",
                Email = "admin@pc.com",
                NormalizedEmail = "admin@pc.com".ToUpper(),
                NormalizedUserName = "admin@pc.com".ToUpper(),

            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Pc@2001");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles to Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId.ToString(),
                    RoleId = readerRoleId.ToString(),
                },

                 new()
                {
                    UserId = adminUserId.ToString(),
                    RoleId = writerRoleId.ToString(),
                },
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);


        }
    }
}
