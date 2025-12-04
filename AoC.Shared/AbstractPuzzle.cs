namespace AoC.Shared;

public abstract class AbstractPuzzle
{
    private readonly AoCHttpClient _client;
    private readonly int _year;
    private readonly int _day;

    internal protected AbstractPuzzle(int year, int day, AoCHttpClient client)
    {
        _client = client;
        _year = year;
        _day = day;

        Console.WriteLine($"Running Advent of Code for year: {_year}, day: {_day}");
    }

    internal protected async Task<string> GetPuzzleInput()
    {
        return await _client.GetPuzzleInput(_year, _day);
    }

    public abstract void Test();
    public abstract Task RunPuzzle();
}
