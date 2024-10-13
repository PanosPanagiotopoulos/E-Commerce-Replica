using E_Commerce_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application_API.Data
{
    /*
     * Here we have the main connection object with the database. Will be used
     *  to interact with the database using Entity Framework Core as Dependency Injection.
     */
    public class DataContext : DbContext  // DbContext is the base class for EF Core DbContext implementations.
    {
        // Constructor to put the data to the Entity Framework
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        /*
        Reference all the tables yoy have in your database (models)
        */
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<MethodProducts> MethodProducts { get; set; }
        public DbSet<CartProducts> CartProducts { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1">DbSet</see> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision for Price and ShippingCost fields
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.ShippingCost)
                .HasColumnType("decimal(18,2)");

            // Configure one-to-one relationship between User and ShoppingCart
            modelBuilder.Entity<User>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(sc => sc.User)
                .HasForeignKey<ShoppingCart>(sc => sc.UserId);

            // Code for handling  many - many join table between Payment Methods and Products //
            modelBuilder.Entity<MethodProducts>()
                .HasKey(mp => new { mp.MethodId, mp.ProductId });
            modelBuilder.Entity<MethodProducts>()
                .HasOne(mp => mp.PaymentMethod)
                .WithMany(mp => mp.MethodProducts)
                .HasForeignKey(c => c.MethodId);

            modelBuilder.Entity<MethodProducts>()
                .HasOne(mp => mp.Product)
                .WithMany(mp => mp.PaymentMethods)
                .HasForeignKey(mp => mp.ProductId);
            // End of Code for handling  many - many join table between Payment Methods and Products //

            modelBuilder.Entity<CartProducts>()
                .HasKey(cp => new { cp.CartId, cp.ProductId });
            modelBuilder.Entity<CartProducts>()
                .HasOne(cp => cp.ShoppingCart)
                .WithMany(cp => cp.CartProducts)
                .HasForeignKey(cp => cp.CartId);

            // Code for handling  many - many join table between Shopping Carts and Products //
            modelBuilder.Entity<CartProducts>()
                .HasOne(cp => cp.Product)
                .WithMany(cp => cp.CartProducts)
                .HasForeignKey(cp => cp.ProductId);
            // End of Code for handling  many - many join table between Payment ShoppingCarts and Products //

            base.OnModelCreating(modelBuilder);

        }
    }
}
