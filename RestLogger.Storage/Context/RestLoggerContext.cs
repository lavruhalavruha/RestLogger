using System.Data.Entity;
using System.Diagnostics;

namespace RestLogger.Storage.Context
{
    internal class RestLoggerContext : DbContext
    {
        public RestLoggerContext() : base("name=RestLoggerContext")
        {
            Database.Log = s => Debug.Write(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
