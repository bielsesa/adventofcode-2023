namespace AdventOfCode.DayOne
{
    public static class DayOne
    {
        private static readonly Dictionary<string, string> DigitsWords = new()
        {
            { "one", "1" }, 
            { "two", "2" }, 
            { "three", "3" }, 
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };
        
        public static void Main(string[] args)
        {
            var calibrationLines = ReadCalibrationDocument();
            var digits = calibrationLines.Select(GetCalibrationDigitsForLine).ToList();
            Console.WriteLine(digits.Sum());
        }

        private static IEnumerable<string> ReadCalibrationDocument()
        {
            var pathToDocument = @$"{Environment.CurrentDirectory}/rsc/calib-doc.txt";
            return File.ReadLines(pathToDocument);
        }

        private static int GetCalibrationDigitsForLine(string line)
        {
            line = line.ReplaceDigitWordsForDigitValue();
            var chars = line.ToCharArray();
            var charsLength = chars.Length;
            
            int firstChar = -1, lastChar = -1;

            for (int i = 0, j = charsLength - 1; i < charsLength && j >= 0; i++, j--)
            {
                if (firstChar != -1 && lastChar != -1)
                {
                    break;
                }
                
                if (firstChar == -1 && char.IsDigit(chars[i]))
                {
                    firstChar = chars[i];
                }
                
                if (lastChar == -1 && char.IsDigit(chars[j]))
                {
                    lastChar = chars[j];
                }
            }

            return int.TryParse(string.Concat((char)firstChar, (char)lastChar), out var digits) ? digits : 0;
        }

        private static string ReplaceDigitWordsForDigitValue(this string line)
        {
            var replacedValues = string.Empty;

            foreach (var item in line.ToList().Select((value, i) => new { i, value}))
            {
                if (char.IsDigit(item.value))
                {
                    replacedValues = string.Concat(replacedValues, item.value);
                    continue;
                }
                
                var j = item.i;
                var digitWord = string.Empty;
                var found = false;
                
                while (!found && j < line.Length && !char.IsDigit(line[j]) && digitWord.Length <= 5) // length > 5 to stop iterating over a "word" that is too long, since the max length for a digit name is 5
                {
                    digitWord = string.Concat(digitWord, line[j]);
                    if (!DigitsWords.TryGetValue(digitWord, out var word))
                    {
                        j++;
                        continue;
                    }
                    replacedValues = string.Concat(replacedValues, word);
                    found = true;
                }
            }

            return replacedValues;
        }
    }
}