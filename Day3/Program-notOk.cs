namespace AdventOfCode.Day3
{
    public static class Program2
    {
        public static void Main2(string[] args)
        {
            var chars = ReadPuzzleInput().ToArray();

            var numbersPerSymbol = new List<List<int>>();
            
            var sum = 0;
            
            for (var i = 0; i < chars.Length; i++)
            {
                for (var j = 0; j < chars[i].Length; j++)
                {
                    var c = chars[i][j];

                    if (c.Equals('.')) continue;

                    if (char.IsDigit(c)) continue;
                    
                    var numbersFromSymbol = new List<int>
                    {
                        CheckIfSquareHasNumber(chars, i, j - 1), 
                        CheckIfSquareHasNumber(chars, i, j + 1),
                        CheckIfSquareHasNumber(chars, i -1, j - 1),
                        CheckIfSquareHasNumber(chars, i + 1, j + 1),
                        CheckIfSquareHasNumber(chars, i + 1, j - 1),
                        CheckIfSquareHasNumber(chars, i - 1, j + 1),
                        CheckIfSquareHasNumber(chars, i - 1, j),
                        CheckIfSquareHasNumber(chars, i + 1, j)
                    };

                    numbersPerSymbol.Add(numbersFromSymbol.Distinct().ToList());
                }
            }

            var distinctParts = numbersPerSymbol.SelectMany(x => x).ToList();
            
            Console.WriteLine(distinctParts.Sum());
        }

        private static int CheckIfSquareHasNumber(IReadOnlyList<char[]> matrix, int i, int j)
        {
            var iSize = matrix.Count;
            if (i < 0 || i >= iSize) return 0;
            var jSize = matrix[i].Length;
            if (j < 0 || j >= jSize) return 0;
            
            var c = matrix[i][j];

            if (!char.IsDigit(c)) 
                return 0;
            
            var possibleNumber = new LinkedList<string>();
            possibleNumber.AddFirst(c.ToString());
            var jpos = j + 1;
            var jneg = j - 1;

            var noMoreAfter = false;
            var noMoreBefore = false;

            for (; possibleNumber.Count < 3 && jneg > 0 && jpos < matrix[i].Length; jpos++, jneg--)
            {
                if (noMoreAfter && noMoreBefore) break;
                
                var after = matrix[i][jpos];
                var before = matrix[i][jneg];
                    
                if (!noMoreAfter && char.IsDigit(after))
                {
                    possibleNumber.AddLast(after.ToString());
                }
                else
                {
                    noMoreAfter = true;
                }
                    
                if (!noMoreBefore && char.IsDigit(before))
                {
                    possibleNumber.AddFirst(before.ToString());
                }
                else
                {
                    noMoreBefore = true;
                }
            }

            var result = int.Parse(string.Join("", possibleNumber));
            
            return result;
        }

        private static IEnumerable<char[]> ReadPuzzleInput()
        {
            var pathToDocument = @$"{Environment.CurrentDirectory}/rsc/input.txt";
            var lines = File.ReadLines(pathToDocument);

            return lines.Select(l => l.ToCharArray());
        }
    }
}