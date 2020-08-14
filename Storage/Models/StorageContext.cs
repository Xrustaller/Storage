using System.Data.Entity;

namespace Storage.Models
{
    public class StorageContext : DbContext
    {
        public StorageContext() : base("DefaultConnection") { }
        public DbSet<Accepted> Accepted { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<SoldOut> SoldOut { get; set; }
    }

}
