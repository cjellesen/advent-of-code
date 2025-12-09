using System.Numerics;
using AoC.Shared;

internal class DayThree : AbstractPuzzle
{

    internal DayThree(AoCHttpClient client) : base(2015, 3, client)
    {
    }

    public override async Task RunPuzzle()
    {
        string puzzleInput = await GetPuzzleInput();
        int nUniqueHouseDeliveries = ParseJourney(puzzleInput);
        Console.WriteLine($"    - Number of house with at least 1 delivery: {nUniqueHouseDeliveries}");
        
        int nUniqueHouseDeliveriesWithCompanion = ParseJourneyWithCompanion(puzzleInput);
        Console.WriteLine($"    - Number of house with at least 1 delivery with companion: {nUniqueHouseDeliveriesWithCompanion}");
    }

    public override void Test()
    {
        foreach ((string journey, int nHouseDeliveries)  in GetPart1TestCases())
        {
            int nUniqueHouseDeliveries = ParseJourney(journey);
            if (nUniqueHouseDeliveries != nHouseDeliveries)
            {
                throw new Exception($"Failed to parse journey: {journey} - Expected {nHouseDeliveries} but got {nUniqueHouseDeliveries}"); 
            }
        }
        
        foreach ((string journey, int nHouseDeliveries)  in GetPart2TestCases())
        {
            int nUniqueHouseDeliveries = ParseJourneyWithCompanion(journey);
            if (nUniqueHouseDeliveries != nHouseDeliveries)
            {
                throw new Exception($"Failed to parse journey: {journey} - Expected {nHouseDeliveries} but got {nUniqueHouseDeliveries}"); 
            }
        }
    }

    private int ParseJourney(string journey)
    {
        var position = new Vector2(0, 0);
        var visited = new HashSet<Vector2> {position};
        
        // We always start at a house
        var nUniqueHouseDeliveries = 1;
        foreach (char direction in journey)
        {
            position += DirectionMapping(direction);
            if (visited.Add(position))
            {
                nUniqueHouseDeliveries++;
            }
        }
        
        return nUniqueHouseDeliveries;
    }
    
    private int ParseJourneyWithCompanion(string journey)
    {
        var stack = new Queue<char>(journey);
        var santaPosition = new Vector2(0, 0);
        var companionPosition = new Vector2(0, 0);
        var visited = new HashSet<Vector2>{santaPosition};

        // We always start at a house
        var nUniqueHouseDeliveries = 1;
        var isSanta = true;
        while (stack.TryDequeue(out char direction))
        {
            if (isSanta)
            {
                santaPosition += DirectionMapping(direction);
                if (visited.Add(santaPosition))
                {
                    nUniqueHouseDeliveries++;
                }
            }
            else
            {
                companionPosition += DirectionMapping(direction);
                if (visited.Add(companionPosition))
                {
                    nUniqueHouseDeliveries++;
                }
            }

            isSanta = !isSanta;
        }
        
        return nUniqueHouseDeliveries;
    }

    private static (string journey, int nHouseDeliveries)[] GetPart1TestCases()
    {
        return [(">", 2), ("^>v<", 4), ("^v^v^v^v^v", 2)];
    }
    
    private static (string journey, int nHouseDeliveries)[] GetPart2TestCases()
    {
        return [("^v", 3), ("^>v<", 3), ("^v^v^v^v^v", 11)];
    }

    private Vector2 DirectionMapping(char direction)
    {
        return direction switch
        {
            '>' => new Vector2(1, 0),
            '<' => new Vector2(-1, 0),
            '^' => new Vector2(0, 1),
            'v' => new Vector2(0, -1),
            _ => throw new ArgumentException($"Invalid direction: '{direction}'")
        };
    }
}

