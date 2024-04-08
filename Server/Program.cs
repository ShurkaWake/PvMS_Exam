using Microsoft.AspNetCore.Mvc;
using Server;

string route = "http://192.168.1.5:4612";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSingleton<QueueService>();
builder.Services.AddSingleton<TictactoeService>();
builder.Services.AddHostedService<QueueHostedService>();

var app = builder.Build();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
});
app.Logger.LogInformation("Server has been started");

app.MapPost("/place", ([FromQuery] bool is_x,
                       [FromQuery] int horizontal,
                       [FromQuery] int vertical,
                       [FromServices] QueueService service
                      )
                         =>
{
    if (horizontal < 0 || vertical < 0 
        || horizontal >= TictactoeService.FIELD_SIZE 
        || vertical >= TictactoeService.FIELD_SIZE)
    {
        return Results.BadRequest($"X and Y must be in range [0; {TictactoeService.FIELD_SIZE})");
    }

    try
    {
        TicTacToe type = TicTacToe.O;
        if (is_x)
        {
            type = TicTacToe.X;
        }
        service.Enqueue(horizontal, vertical, type);
    }
    catch (Exception ex)
    {
        app.Logger.LogError($"Error: {ex.Message}");
        return Results.BadRequest("An error occured. Try again later");
    }
    return Results.Accepted();
});

app.MapGet("/state", ([FromServices] TictactoeService service) =>
{
    TicTacToe winner = service.GetWinner();
    var moves = service.GetField();
    var result = new TictactoeStateModel
    {
        Winner = winner,
        Moves = moves
    };
    return Results.Ok(result);
});

app.Run(route);