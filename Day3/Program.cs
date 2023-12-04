using Helpers;

namespace AdventOfCode.Day3
{
    public class HitPoint
    {
        public string Value { get; set; } = string.Empty;
        public (int y, int x) Position { get; set; } = new(-1, -1);
    }
    
    public abstract class EngineThing
    {
        public Tuple<int,int> FirstPosition { get; set; } = new(-1,-1);
        public Tuple<int, int> LastPosition { get; set; } = new(-1,-1);
        public List<HitPoint> HitPoints { get; set; } = new();
    }
    
    public class EnginePart : EngineThing
    {
        public int Number { get; set; }
    }
    
    public class EngineGear : EngineThing
    {
        public int GearRatio { get; set; }
    }
    
    public static class Program
    {
        public static void Main(string[] args)
        {
            var matrix = FilesHelper.ReadPuzzleInputToMatrix();
            var (engineParts, engineGears) = AnalyzeEngineDocument(matrix);
            Console.WriteLine($"Engine parts sum: {engineParts.Sum(part => part.Number)}");
            Console.WriteLine($"Engine gear ratio sum: {engineGears.Sum(gear => gear.GearRatio)}");
        }

        private static (IEnumerable<EnginePart> engineParts, IEnumerable<EngineGear> engineGears) AnalyzeEngineDocument(char[][] matrix)
        {
            var validEngineParts = new List<EnginePart>();
            var engineGears = new List<EngineGear>();
            
            for (var y = 0; y < matrix.Length; y++)
            {
                for (var x = 0; x < matrix.Length;)
                {
                    if (char.IsDigit(matrix[y][x]))
                    {
                        var enginePart = GetEnginePart(matrix, y, x);
                        if (enginePart.HitPoints.Any(hp => IsValidSymbol(hp.Value)))
                        {
                            validEngineParts.Add(enginePart);
                        }

                        x = enginePart.LastPosition.Item2;
                    }
                    else
                    {
                        if (matrix[y][x].Equals('*'))
                        {
                            engineGears.Add(GetEngineGear(matrix, y, x));
                        }
                    }
                    x++;
                }
            }
            
            return (validEngineParts, engineGears);
        }

        private static EnginePart GetEnginePart(char[][] matrix, int y, int x)
        {
            var line = matrix[y];
            var number = new List<char> { line[x] };
            
            var enginePart = new EnginePart
            {
                FirstPosition = new Tuple<int, int>(y, x)
            };

            x++;
            for (; number.Count < 3 && x < line.Length; x++)
            {
                if (!char.IsDigit(line[x])) break;
                
                number.Add(line[x]);
            }

            enginePart.Number = int.Parse(string.Join(string.Empty, number));
            enginePart.LastPosition = new Tuple<int, int>(y, x-1);
            enginePart.HitPoints = GetHitPoints(matrix, enginePart);
            
            return enginePart;
        }

        private static EngineGear GetEngineGear(char[][] matrix, int y, int x)
        {
            var line = matrix[y];
            
            var engineGear = new EngineGear
            {
                FirstPosition = new Tuple<int, int>(y, x),
                LastPosition = new Tuple<int, int>(y, x)
            };
            
            engineGear.HitPoints = GetHitPoints(matrix, engineGear);
            engineGear.GearRatio = GetGearRatio(matrix, engineGear);
            
            return engineGear;
        }

        private static int GetGearRatio(char[][] matrix, EngineGear gear)
        {
            var gearRatio = 0;

            var gearDigits = new List<int>();
            
            foreach (var hitPoint in gear.HitPoints)
            {
                if (string.IsNullOrWhiteSpace(hitPoint.Value)) continue;
                var digit = hitPoint.Value.ToCharArray()[0];
                if (!char.IsDigit(digit)) continue;
                
                gearDigits.Add(GetFullGearDigit(matrix, hitPoint));
            }

            gearDigits = gearDigits.Distinct().ToList();

            if (gearDigits.Count == 2)
            {
                gearRatio = gearDigits.Aggregate(1, (num1, num2) => num1 * num2);
            }
            
            return gearRatio;
        }

        private static int GetFullGearDigit(char[][] matrix, HitPoint hitPoint)
        {
            var line = matrix[hitPoint.Position.y];
            var x = hitPoint.Position.x;
            var number = new LinkedList<string>();
            number.AddFirst(line[x].ToString());
            
            var xLeft = x - 1;
            while (xLeft >= 0 && char.IsDigit(line[xLeft]))
            {
                number.AddFirst(line[xLeft--].ToString());
            }
            
            var xRight = x + 1;
            while (xRight < line.Length && char.IsDigit(line[xRight]))
            {
                number.AddLast(line[xRight++].ToString());
            }
            
            return int.Parse(string.Join(string.Empty, number));
        }

        private static List<HitPoint> GetHitPoints(char[][] matrix, EngineThing engineThing)
        {
            var y = engineThing.FirstPosition.Item1;
            var leftX = engineThing.FirstPosition.Item2;
            var rightX = engineThing.LastPosition.Item2;

            var hitPoints = new List<HitPoint>();

            for (var x = leftX; x <= rightX+1; x++)
            {
                hitPoints.Add(GetHitPoint(matrix, y-1, x));
                hitPoints.Add(GetHitPoint(matrix, y+1, x));
            }
            
            hitPoints.Add(GetHitPoint(matrix, y-1, leftX-1));
            hitPoints.Add(GetHitPoint(matrix, y, leftX-1));
            hitPoints.Add(GetHitPoint(matrix, y+1, leftX-1));
            hitPoints.Add(GetHitPoint(matrix, y-1, rightX+1));
            hitPoints.Add(GetHitPoint(matrix, y, rightX+1));
            
            return hitPoints;
        }

        private static HitPoint GetHitPoint(char[][] matrix, int y, int x)
        {
            if (y < 0 || y >= matrix.Length 
                || x < 0 || x >= matrix[y].Length)
            {
                return new HitPoint();
            }

            return 
            new HitPoint{
                Position = (y, x),
                Value = matrix[y][x].ToString()
            };
        }

        private static bool IsValidSymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol)) return false;
            var character = symbol.ToCharArray()[0];
            return !char.IsDigit(character) && !character.Equals('.');
        }
    }
}