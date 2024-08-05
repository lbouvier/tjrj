using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace TJRJ.Web.Data
{
    public class ContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost;Port=3306;Database=tjrj;Uid=root;Pwd=123456";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var server = ServerVersion.AutoDetect(connectionString).Version;
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
