using Helpers;

namespace AdventOfCode;

// Using Michael Tezak's solutions.
// Their AoC amazing webpage: https://aoc-puzzle-solver.streamlit.app/
public static class Day9
{
    public static void Main(string[] args)
    {
        var input = FilesHelper.ReadPuzzleInputToLines();
        var totalFirst = FirstPart(input);
        var totalSecond = SecondPart(input);

        Console.WriteLine($"Total part 1: {totalFirst}");
        Console.WriteLine($"Total part 2: {totalSecond}");
    }

    private static int FirstPart(IEnumerable<string> input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var nums = line.Split().Select(int.Parse).ToList();
            var finalNums = new List<int>();

            while (nums.Any(x => x != 0))
            {
                finalNums.Add(nums.Last());

                var newNums = new List<int>();
                for (var i = 1; i < nums.Count; i++)
                {
                    newNums.Add(nums[i] - nums[i - 1]);
                }

                nums = new List<int>(newNums);
            }

            total += finalNums.Sum();
        }

        return total;
    }

    private static object SecondPart(IEnumerable<string> input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var nums = line.Split().Select(int.Parse).ToList();
            var firstNums = new List<int>();

            while (nums.Any(x => x != 0))
            {
                firstNums.Add(nums.First());

                var newNums = new List<int>();
                for (var i = 1; i < nums.Count; i++)
                {
                    newNums.Add(nums[i] - nums[i - 1]);
                }

                nums = new List<int>(newNums);
            }
            
            total += firstNums.Select((num, i) => i % 2 == 0 ? num : -num).Sum();
        }

        return total;
    }
}