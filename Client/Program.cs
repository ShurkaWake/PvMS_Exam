// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using System.Web;

namespace Client;

internal class Program
{
    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://192.168.1.5:4612/"),
    };
    
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello this is simple tic tac toe game. ");
        bool outflag = false;
        while(true)
        {
            Console.WriteLine(
            "\nChoose what do you want: " +
            "\n1 - make moves" +
            "\n2 - check board" +
            "\n3 - close app)");

            var input = Console.ReadLine();

            switch (input) 
            {
                case "1":
                    Console.WriteLine("\nType a move (1 for X; 2 for O) in by example: 1 11 22");
                    var move = Console.ReadLine().Split().Select(n => Convert.ToInt32(n)).ToArray();
                    await Place(move[1], move[2], (TicTacToe) move[0]);
                    break;
                case "2":
                    await Update();
                    break;
                case "3":
                    outflag = false;
                    break;
                default:
                    Console.WriteLine("\nUnexpected input");
                    break;
            }
        }
        
        await Place(0, 1, TicTacToe.X);
        await Update();
        Console.ReadLine();
    }

    public static async Task Place(int x, int y, TicTacToe move)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["is_x"] = (move == TicTacToe.X).ToString().ToLower();
        query["horizontal"] = x.ToString();
        query["vertical"] = y.ToString();

        var uri = new UriBuilder(httpClient.BaseAddress)
        {
            Path = "/place",
            Query = query.ToString(),
        };

        var response = await httpClient.PostAsync(uri.Uri, null);
        Console.WriteLine($"\n({response.StatusCode})");
    }

    public static async Task Update()
    {
        var response = await httpClient.GetFromJsonAsync<TictactoeStateModel>("/state");
        Console.WriteLine($"\nWinner: {response.Winner}");
        Console.WriteLine($"Current moves: ");
        foreach (var elem in response.Moves)
        {
            Console.WriteLine($"{elem.Type} x={elem.x} y={elem.y}");
        }
    }
}

