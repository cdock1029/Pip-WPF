using System.Diagnostics;
using System.Reflection;

namespace Pip.DataAccess;

public static class DatabasePathHelper
{
    public static string GetDatabasePath(string dbName)
    {
        string? caller = Assembly.GetEntryAssembly()?.GetName().Name;

        if (caller is null) return dbName;

        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string appFolder = Path.Combine(localAppData, caller);

        if (!Directory.Exists(appFolder))
            try
            {
                Directory.CreateDirectory(appFolder);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error creating app directory {appFolder}.\n{e.Message}");
                throw;
            }

        return Path.Combine(appFolder, dbName);
    }
}