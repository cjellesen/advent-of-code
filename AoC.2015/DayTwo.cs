using System.Text;
using AoC.Shared;

internal class DayTwo : AbstractPuzzle
{

    internal DayTwo(AoCHttpClient client) : base(2015, 2, client)
    {
    }

    public override async Task RunPuzzle()
    {
        string puzzleInput = await GetPuzzleInput();
        int totalWrapPaper = 0;
        int totalRibbonLength = 0;
        foreach (string dimension in puzzleInput.Split("\n"))
        {
            if (dimension.Length == 0)
            {
                break;
            }
            (int length, int width, int height) = ParseDimension(dimension);
            var prism = new RectangularPrism(length, width, height);
            totalWrapPaper += prism.ComputeArea() + prism.GetSmallestFaceArea();
            totalRibbonLength += prism.GetSmallestPerimiterLength() + prism.ComputeVolume();
        }

        Console.WriteLine($"    - Total area of wrapping paper needed: {totalWrapPaper}");
        Console.WriteLine($"    - Total length of ribbon needed: {totalRibbonLength}");
    }

    public override void Test()
    {
        foreach ((string dimension, int area, int wrappingAmount, int wrapRibbonLength, int bowRibbonLength) in GetTestCases())
        {
            (int length, int width, int height) = ParseDimension(dimension);
            var prism = new RectangularPrism(length, width, height);
            Console.WriteLine($"    - Created the test prism of: {prism.Length}x{prism.Width}x{prism.Height} from the input string {dimension}");

            int prismArea = prism.ComputeArea();
            if (prismArea != area)
            {
                throw new Exception($"Failed to correctly determine area of the prism: '{dimension}' - Expected an area of {area} but got {prismArea}");
            }

            int prismWrapAmount = prism.ComputeArea() + prism.GetSmallestFaceArea();
            if (prismWrapAmount != wrappingAmount)
            {
                throw new Exception($"Failed to correctly determine amount of wrapping paper of the prism: '{dimension}' - Expected a wrapping amount of {wrappingAmount} but got {prismWrapAmount}");
            }

            int prismSmallestCircumference = prism.GetSmallestPerimiterLength();
            if (prismSmallestCircumference != wrapRibbonLength)
            {
                throw new Exception($"Failed to correctly determine the smallest circumference of the prism: '{dimension}' - Expected a circumference of {wrapRibbonLength} but got {prismSmallestCircumference}");
            }

            int prismVolume = prism.ComputeVolume();
            if (prismVolume != bowRibbonLength)
            {
                throw new Exception($"Failed to correctly determine the volume of the prism: '{dimension}' - Expected a volume of {bowRibbonLength} but got {prismVolume}");
            }
        }
    }

    private static (string dimension, int area, int wrappingAmount, int wrapRibbonLength, int bowRibbonLength)[] GetTestCases()
    {
        return [("2x3x4", 52, 58, 10, 24), ("1x1x10", 42, 43, 4, 10)];
    }

    private readonly StringBuilder stringBuilder = new();

    private (int length, int width, int height) ParseDimension(ReadOnlySpan<char> dimensions)
    {
        stringBuilder.Clear();
        int length = 0;
        int width = 0;
        int i = 0;
        foreach (char item in dimensions)
        {
            if (item == 'x')
            {
                i++;
                int value = int.Parse(stringBuilder.ToString());
                switch (i)
                {
                    case 1:
                        length = value;
                        break;
                    case 2:
                        width = value;
                        break;
                    default:
                        throw new ArgumentException($"Failed to correctly parse the dimension '{dimensions}'");
                }

                stringBuilder.Clear();
                continue;
            }

            if (!char.IsDigit(item))
            {
                throw new Exception($"Failed to parse the input dimension of '{dimensions}' - Expected the {i}th character ('{item}') to be a digit but it wasn't");
            }

            stringBuilder.Append(item);
        }

        if (i != 2)
        {
            throw new Exception($"Expected to encounter 2 delimiters ('x') in the input dimensions '{dimensions}' but encoutered {i}");
        }

        if (stringBuilder.Length == 0)
        {
            throw new Exception($"Expected a remaining value after all delimiters have been parsed but this was not the case - Something wen't wrong");
        }

        int height = int.Parse(stringBuilder.ToString());
        stringBuilder.Clear();

        return (length, width, height);
    }

    private record RectangularPrism
    {
        internal readonly int Height;
        internal readonly int Length;
        internal readonly int Width;

        internal RectangularPrism(int length, int width, int height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        internal int ComputeVolume()
        {
            return Length * Width * Height;
        }

        internal int ComputeArea()
        {
            return 2 * ComputeSideArea() + 2 * ComputeWidthFaceArea() + 2 * ComputeLengthFaceArea();
        }

        private int ComputeSideArea()
        {
            return Length * Width;
        }

        private int ComputeLengthFaceArea()
        {
            return Length * Height;
        }

        private int ComputeWidthFaceArea()
        {
            return Width * Height;
        }

        internal int GetSmallestFaceArea()
        {
            return Math.Min(Math.Min(ComputeLengthFaceArea(), ComputeWidthFaceArea()), ComputeSideArea());
        }

        internal int GetSmallestPerimiterLength()
        {
            int lengthFacePerimiter = Length + Height;
            int widthFacePerimiter = Width + Height;
            int sidePerimiter = Length + Width;
            return 2 * Math.Min(Math.Min(lengthFacePerimiter, widthFacePerimiter), sidePerimiter);
        }
    }
}

