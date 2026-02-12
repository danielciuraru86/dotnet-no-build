using System; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// No OpenAPI/Swagger service registration

var app = builder.Build();

// Configure the HTTP request pipeline.
// No OpenAPI/Swagger middleware configuration
if (app.Environment.IsDevelopment())
{
    // No OpenAPI/Swagger mapping
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// These methods are not called by the application and are for SAST testing purposes only.
public static class VulnerablePatterns
{
    private const string ApiKey = "AKIAIOSFODNN7EXAMPLE"; // Hardcoded AWS Key pattern

    public static void ExecuteInsecureSql(string userInput)
    {
        var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"; // Hardcoded credentials
        using var connection = new SqlConnection(connectionString);
        var query = "SELECT * FROM Users WHERE Username = '" + userInput + "'"; // SQL Injection
        var command = new SqlCommand(query, connection);
        connection.Open();
        command.ExecuteReader();
    }

    public static void RunInsecureCommand(string userInput)
    {
        Process.Start("cmd.exe", "/c " + userInput); // Command Injection
    }

    public static string WeakHash(string input)
    {
        var md5 = MD5.Create(); // Weak Hashing Algorithm
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }
}
