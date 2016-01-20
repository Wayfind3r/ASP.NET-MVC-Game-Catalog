using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PotatoCatalog.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser<int, UserLogin, UserRole,UserClaim>
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Potatoes { get; set; } = 300;
        public virtual ICollection<Order> Orders { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public
class ApplicationDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole,UserClaim>
{
        public ApplicationDbContext(): base("DefaultConnection")
        { }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<TagItem> TagItems { get; set; }
        public virtual DbSet<Edition> Editions { get; set; }
        public virtual DbSet<GameEdition> GameEditions { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(
            modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity < UserLogin > ().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Game>().ToTable("Games");
            modelBuilder.Entity<TagItem>().ToTable("TagItems");
            modelBuilder.Entity<Edition>().ToTable("Editions");
            modelBuilder.Entity<GameEdition>().ToTable("GameEditions");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
        }
        public static ApplicationDbContext Create(){return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<PotatoCatalog.Models.UserTableViewModel> UserTableViewModels { get; set; }
    }
}