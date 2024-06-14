using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using System.Windows;
using Pip.Model;

namespace Pip;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        await GetData();
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }

    private static void ExcludeEmptyStrings(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (var jsonPropertyInfo in jsonTypeInfo.Properties)
            if (jsonPropertyInfo.PropertyType == typeof(string))
                jsonPropertyInfo.ShouldSerialize = static (obj, value) =>
                    !string.IsNullOrEmpty(value as string);
    }

    private static async Task GetData()
    {
        using HttpClient client = new()
        {
            BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/")
        };

        var treasury =
            await client.GetFromJsonAsync<Treasury>(
                "securities/912797KL0/6/20/2024?format=json");

        Debug.WriteLine($"Cusip: {treasury?.Cusip}");
        Debug.WriteLine($"Issue Date: {treasury?.IssueDate}");
        Debug.WriteLine($"Maturity Date: {treasury?.MaturingDate}");
    }
}