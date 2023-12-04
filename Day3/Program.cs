using Helpers;

namespace AdventOfCode.Day3
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var chars = FilesHelper.ReadPuzzleInputToMatrix();
            Console.WriteLine(GetPartsOfEngine(chars).Sum());
        }

        private static IEnumerable<int> GetPartsOfEngine(IReadOnlyList<char[]> matrix)
        {
            var parts = new List<int>();

            for (var i = 0; i < matrix.Count; i++)
            {
                for (var j = 0; j < matrix[i].Length;)
                {
                    var c = matrix[i][j];
                    if (!char.IsDigit(c))
                    {
                        j++;
                        continue;
                    }

                    var fullDigit = GetFullDigit(matrix[i], j);
                    if (!SurroundingSquareHasSymbol(matrix, fullDigit, i, j))
                    {
                        j += fullDigit.Length;
                        continue;
                    }
                    
                    parts.Add(int.Parse(fullDigit));
                    j += fullDigit.Length;
                }
            }

            return parts;
        }

        private static string GetFullDigit(IReadOnlyList<char> line, int j)
        {
            if (j < 0 || j >= line.Count) return "0";

            var number = new List<char> { line[j] };
            
            for (var nextChar = j + 1; number.Count < 3 && nextChar < line.Count; nextChar++)
            {
                if (!char.IsDigit(line[nextChar])) break;
                
                number.Add(line[nextChar]);
            }
            
            var result = string.Join(string.Empty, number);
            return result;
        }

        private static bool SurroundingSquareHasSymbol(IReadOnlyList<char[]> matrix, string number, int x, int y)
        {
            var rowLength = matrix[x].Length;
            var numberLength = number.Length;
            
            // current row (i)
            if (y - 1 >= 0 && IsValidSymbol(matrix[x][y - 1]))
                return true;
            if (y + numberLength < rowLength && IsValidSymbol(matrix[x][y + numberLength]))
                return true;
                
            var maxIterations = numberLength + 2;
            var initColumn = y == 0 ? y : y - 1;
            var w = initColumn;
            var iterations = 0;
            
            // previous row (i - 1)
            if (x - 1 >= 0)
            {
                for (;w < rowLength && iterations < maxIterations; w++, iterations++)
                {
                    var c = matrix[x - 1][w];
                    if (IsValidSymbol(c))
                        return true;
                }
            }
            
            // next row (i + 1)
            if (x >= matrix.Count - 1) 
                return false;
            
            w = initColumn;
            iterations = 0;
            for (; w < rowLength && iterations < maxIterations; w++, iterations++)
            {
                var c = matrix[x + 1][w];
                if (IsValidSymbol(c))
                    return true;
            }
            
            return false;
        }

        private static bool IsValidSymbol(char c)
        {
            return !char.IsDigit(c) && !c.Equals('.');
        }
    }
}