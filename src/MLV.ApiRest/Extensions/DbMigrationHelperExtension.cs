using MLV.ApiRest.Configuration;

namespace MLV.ApiRest.Extensions;

public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        DbMigrationHelper.EnsureSeedData(app).Wait();
    }
}
