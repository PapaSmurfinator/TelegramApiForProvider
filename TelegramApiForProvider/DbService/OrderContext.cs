using Microsoft.EntityFrameworkCore;
using TelegramApiForProvider.Models;

namespace TelegramApiForProvider.DbService
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderMessage> OrderMessages { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public OrderContext() : base()
        {

        }
    }
}
