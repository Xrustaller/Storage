using System.Data.Entity;
using Storage.Database;

namespace Storage.Models
{
    public class StorageContext : DbContext
    {
        public StorageContext()
        {

        }
        public StorageContext(string dbName) : base(dbName)
        {
            System.Data.Entity.Database.SetInitializer(new DataBaseInitializer());
        }
        public DbSet<Accepted> Accepted { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<SoldOut> SoldOut { get; set; }
    }

}
