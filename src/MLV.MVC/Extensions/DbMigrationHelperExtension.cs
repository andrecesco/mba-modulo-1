using MLV.MVC.Configuration;

namespace MLV.MVC.Extensions;

public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        DbMigrationHelper.EnsureSeedData(app).Wait();
    }
}
