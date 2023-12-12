using System.Text;

namespace Helpers
{
    public static class FilesHelper
    {
        private static readonly string PathToInput = @$"{Environment.CurrentDirectory}/rsc/input.txt";
        private static readonly string PathToOutput = @$"{Environment.CurrentDirectory}/rsc/output.txt";
        
        public static void WriteLinesToOutput(IEnumerable<string> lines)
        {
            using var fs = File.Create(PathToOutput);
            
            var info = new UTF8Encoding(true).GetBytes(string.Join(Environment.NewLine, lines));
            fs.Write(info, 0, info.Length);
        }
        
        public static IEnumerable<string> ReadPuzzleInputToLines()
        {
            return File.ReadLines(PathToInput);
        }
        
        public static Stack<string> ReadPuzzleInputToStack()
        {
            var stack = new Stack<string>();
            var lines = File.ReadLines(PathToInput);

            foreach (var line in lines.Reverse())
            {
                stack.Push(line);
            }
            
            return stack;
        }
        
        public static char[][] ReadPuzzleInputToMatrix()
        {
            var lines = File.ReadLines(PathToInput);
            return lines.Select(l => l.ToCharArray()).ToArray();
        }
    }
}

