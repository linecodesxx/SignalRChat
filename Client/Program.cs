using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Введите свое имя: ");
var username = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(username))
{
    Console.WriteLine("Имя пользователя не может быть пустым.");
    return;
}

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/chat")
    .WithAutomaticReconnect()
    .Build();

connection.On<string>("Receive", message =>
{
    Console.WriteLine($"\n{message}");
    Console.Write("> ");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Вы подключились к чату.");
    Console.WriteLine("Введите /exit для выхода.");

    while (true)
    {
        Console.Write("> ");
        var message = Console.ReadLine();

        if (message is null ||
            message.Equals("/exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            continue;
        }

        await connection.InvokeAsync("Send", username, message);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
finally
{
    await connection.DisposeAsync();
}