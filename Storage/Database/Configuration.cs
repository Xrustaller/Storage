using Storage.Models;
using System.Data.Entity.Migrations;

namespace Storage.Database
{
    public sealed class Configuration : DbMigrationsConfiguration<StorageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StorageContext context)
        {

        }
    }
}
