using Storage.Models;
using System.Data.Entity;

namespace Storage.Database
{
    public class DataBaseInitializer : MigrateDatabaseToLatestVersion<StorageContext, Configuration>
    {

    }
}
