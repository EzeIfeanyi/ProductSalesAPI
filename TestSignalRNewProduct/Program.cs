using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

Console.ReadLine();

var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7212/notificationsHub")
            .AddJsonProtocol()
            .Build();

connection.On<string, decimal>("ReceiveNewProduct", (name, price) =>
{
    Console.WriteLine($"New product created: {name}, Price: {price}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to SignalR hub with JSON protocol. Listening for messages...");

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
}