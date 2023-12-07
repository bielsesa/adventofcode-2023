using Helpers;

namespace AdventOfCode.Day6;

public static class Day7
{
    public static void Main(string[] args)
    {
        var input = FilesHelper.ReadPuzzleInputToLines();
        var hands = input.Select(Hand.ExtractHandFromString).ToList();
        hands.Sort();

        var total = hands.Select((t, i) => t.Bid * (i + 1)).Sum();

        FilesHelper.WriteLinesToOutput(hands.Select(h => h.ToString()));
        Console.WriteLine($"{Environment.NewLine}Total winnings: {total}");
    }
}