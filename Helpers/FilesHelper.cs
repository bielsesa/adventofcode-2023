namespace Helpers
{
    public static class FilesHelper
    {
        private static string pathToInput = @$"{Environment.CurrentDirectory}/rsc/input.txt";
        
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

