using System.Text;

namespace Helpers
{
    public static class FilesHelper
    {
        private static string pathToInput = @$"{Environment.CurrentDirectory}/rsc/input.txt";
        private static string pathToOutput = @$"{Environment.CurrentDirectory}/rsc/output.txt";
        
        public static void WriteLinesToOutput(IEnumerable<string> lines)
        {
            using var fs = File.Create(pathToOutput);
            
            // writing data in string
            var info = new UTF8Encoding(true).GetBytes(string.Join(string.Empty, lines));
            fs.Write(info, 0, info.Length);
        }
        
        public static IEnumerable<string> ReadPuzzleInputToLines()
        {
            return File.ReadLines(pathToInput);
        }
        
        public static char[][] ReadPuzzleInputToMatrix()
        {
            var lines = File.ReadLines(pathToInput);
            return lines.Select(l => l.ToCharArray()).ToArray();
        }
    }
}

