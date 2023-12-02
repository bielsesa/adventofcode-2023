namespace AdventOfCode.DayTwo
{
    public static class DayTwo
    {
        private static readonly Dictionary<string, int> possibleMaxCubes = new()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };
        
        public static void Main(string[] args)
        {
            var games = ReadPuzzleInput().ToList();
            var sumOfGameIds = GetIdsOfPossibleGames(games).Sum();
            Console.WriteLine(sumOfGameIds);
            var fullPower = GetSumOfAllGameSetsPowers(games);
            Console.WriteLine(fullPower);
        }

        private static IEnumerable<string> ReadPuzzleInput()
        {
            var pathToDocument = @$"{Environment.CurrentDirectory}/rsc/input.txt";
            return File.ReadLines(pathToDocument);
        }

        private static IEnumerable<int> GetIdsOfPossibleGames(IEnumerable<string> games)
        {
            return games.Select(game =>
            {
                var (isPossible, gameId) = IsGamePossible(game);
                return isPossible ? gameId : 0;
            });
        }

        private static (bool IsPossible, int GameId) IsGamePossible(string game)
        {
            var isGamePossible = true;
            var gameSeparated = game.Split(':');
            var (gameCombinations, gameId) = 
            (
                gameSeparated[1].Trim().Split(';'), 
                int.Parse(gameSeparated[0].Replace("Game", string.Empty).Trim())
            );

            foreach (var combination in gameCombinations)
            {
                var cubeColors = combination.Split(',');

                foreach (var cubeColor in cubeColors)
                {
                    var cubeColorSplit = cubeColor.Trim().Split();
                    var (cubes, color) = (int.Parse(cubeColorSplit[0]), cubeColorSplit[1]);

                    if (color.Contains("red"))
                    {
                        if (cubes > possibleMaxCubes["red"])
                        {
                            isGamePossible = false;
                            break;
                        }
                    }
                    
                    if (color.Contains("green"))
                    {
                        if (cubes > possibleMaxCubes["green"])
                        {
                            isGamePossible = false;
                            break;
                        }
                        
                    }

                    if (!color.Contains("blue")) continue;

                    if (cubes <= possibleMaxCubes["blue"]) continue;
                    isGamePossible = false;
                    break;
                }
            }
            
            return (isGamePossible, gameId);
        }

        // For each game, find the minimum set of cubes that must have been present.
        // What is the sum of the power of these sets?
        private static int GetSumOfAllGameSetsPowers(IEnumerable<string> games)
        {
            return games.Select(GetGameSetPower).Sum();
        }

        // The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together.
        private static int GetGameSetPower(string game)
        {
            var gameCombinations = game.Split(':')[1].Trim().Split(';');
            var maxRed = 0;
            var maxGreen = 0;
            var maxBlue = 0;

            foreach (var combination in gameCombinations)
            {
                var cubeColors = combination.Split(',');

                foreach (var cubeColor in cubeColors)
                {
                    var cubeColorSplit = cubeColor.Trim().Split();
                    var (cubes, color) = (int.Parse(cubeColorSplit[0]), cubeColorSplit[1]);

                    if (color.Contains("red") && cubes > maxRed)
                    {
                        maxRed = cubes;
                    }
                
                    if (color.Contains("green") && cubes > maxGreen)
                    {
                        maxGreen = cubes;
                    }

                    if (!color.Contains("blue")) continue;

                    if (cubes > maxBlue)
                    {
                        maxBlue = cubes;
                    }
                }
            }

            return maxRed * maxGreen * maxBlue;
        }
    } 
}