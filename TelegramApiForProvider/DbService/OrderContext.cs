using Microsoft.EntityFrameworkCore;
using TelegramApiForProvider.Extended;
using TelegramApiForProvider.Models;

namespace TelegramApiForProvider.DbService
{
    public class OrderContext : DbContext
    {
        public DbSet<ExtendedOrder> ExtendedOrders { get; set; }
        public DbSet<User> Users { get; set; }

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
