namespace AoC.Shared;


public class AoCHttpClient
{
    private readonly static SocketsHttpHandler _socketHttpHandler = new()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(15),
    };

    private static readonly HttpClient _client = new(_socketHttpHandler)
    {
    };

    public AoCHttpClient(string sessionId)
    {
        _client.BaseAddress = new Uri("https://www.adventofcode.com/");
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:145.0) Gecko/20100101 Firefox/145.0");
        _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        _client.DefaultRequestHeaders.Add("Cookie", $"session={sessionId}");
    }

    private static string GenerateCachePathForPuzzleInput(int year, int day)
    {
        return Path.Combine(Path.GetTempPath(), "AoC", year.ToString(), $"Day_{day}.txt");
    }

    public async Task<string> GetPuzzleInput(int year, int day)
    {
        string cachePath = GenerateCachePathForPuzzleInput(year, day);
        if (File.Exists(cachePath))
        {
            Console.WriteLine($"    - Found cached puzzle input for year: {year}, day {day}");
            await using (var stream = new FileStream(cachePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        Console.WriteLine($"    - No cached puzzle input found for year: {year}, day {day} - Making request to Advent of Code");
        HttpResponseMessage test = await _client.GetAsync($"/{year}/day/{day}/input");
        if (!test.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to retrieve puzzle input for year: {year}, day: {day} - Failed due to: {test.ReasonPhrase}");
        }

        string? content = await test.Content.ReadAsStringAsync();
        if (content is null)
        {
            throw new NullReferenceException($"The content for the puzzle input for year: {year}, day: {day} was null");
        }

        if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(cachePath) ?? throw new NullReferenceException($"Failed to determine a valid directory path for {cachePath}"));
        }

        Console.WriteLine($"Creating a cache file at '{cachePath}'' for the puzzle input");
        await using (var stream = new FileStream(cachePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(content);
            }
        }

        return content;
    }
}
