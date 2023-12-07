using Helpers;

namespace AdventOfCode.Day6
{
    public class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }
    }
    
    public static class Day6
    {
        public static void Main(string[] args)
        {
            var input = FilesHelper.ReadPuzzleInputToLines().ToList();
            var races = GetRaces(input[0], input[1]);

            var numberOfWaysToWin =
                races.Select(GetNumberOfPossibleWinsFromRace).Aggregate(1, (num1, num2) => (int)(num1 * num2));
            
            Console.WriteLine($"Number of ways (for various races): {numberOfWaysToWin}");

            var race = GetUniqueRace(input[0], input[1]);
            numberOfWaysToWin = (int)GetNumberOfPossibleWinsFromRace(race);
            
            Console.WriteLine($"Number of ways (for the unique race): {numberOfWaysToWin}");
        }

        private static long GetNumberOfPossibleWinsFromRace(Race race)
        {
            var time = 1L;
            // 0 and length do not give you any winning possibilities (bc you dont move at all)
            for (; time < race.Time; time++)
            {
                // you wait { time } ms.
                // then the boat will travel { time } mm/ms
                // for { race.Time - time } ms.

                var distance = time * (race.Time - time);
                if (distance > race.Distance)
                    break;
            }

            var maxTimeToWin = race.Time - time;

            return maxTimeToWin + 1 - time;
        }

        private static Race GetUniqueRace(string times, string distances)
        {
            var (timesArray, distancesArray) = GetTimesAndDistancesAsInt(times, distances);

            var time = long.Parse(string.Join(string.Empty, timesArray.Select(a => a.ToString())));
            var distance = long.Parse(string.Join(string.Empty, distancesArray.Select(a => a.ToString())));

            return new Race
            {
                Distance = distance,
                Time = time
            };
        }

        private static IEnumerable<Race> GetRaces(string times, string distances)
        {
            var (timesArray, distancesArray) = GetTimesAndDistancesAsInt(times, distances);
            
            return timesArray.Select((time, index) =>
                new Race { Time = time, Distance = distancesArray[index] });
        }

        private static (List<long> TimesArray, List<long> DistanceArray) GetTimesAndDistancesAsInt(
            string times, string distances)
        {
            
            var cleanTimes = times.Replace("Time:", string.Empty).Trim();
            var cleanDistances = distances.Replace("Distance:", string.Empty).Trim();

            var timesArray = cleanTimes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var distancesArray = cleanDistances.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            return (timesArray, distancesArray);
        }
    }
}