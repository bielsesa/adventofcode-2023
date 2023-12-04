using System.Diagnostics;
using System.Text.RegularExpressions;
using Helpers;

namespace AdventOfCode.Day4
{
    public class Card
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int NumberOfCopies { get; set; } = 1;
        public IEnumerable<int> WinningNumbers { get; set; }
        public IEnumerable<int> CardNumbers { get; set; }
        public IEnumerable<int> CopyCardsIds { get; set; }
    }
        
    public static partial class Program
    {
        
        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var input = FilesHelper.ReadPuzzleInputToLines();
            var cards = GetCardsFromInput(input).ToList();
            var finalCards = ProcessCopiesOfCards(cards);
            stopwatch.Stop();
            
            Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine($"Sum of total points: {cards.Sum(card => card.Points)}");
            Console.WriteLine($"Final quantity of cards: {finalCards.Sum(card => card.NumberOfCopies)}");
        }

        private static IEnumerable<Card> GetCardsFromInput(IEnumerable<string> input)
        {
            return input.Select(GetCard);
        }

        private static Card GetCard(string cardLine)
        {
            var splitByColon = cardLine.Split(":", StringSplitOptions.TrimEntries);

            var cardId = int.Parse(DigitRegex().Match(splitByColon[0]).Value);
            
            var allNumbersInCard = splitByColon[1].Split('|', StringSplitOptions.TrimEntries);
            var winningNumbers = allNumbersInCard[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var cardNumbers = allNumbersInCard[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var intersectingNumbers = winningNumbers.Intersect(cardNumbers).ToList();

            var copyCardsIds = new List<int>();
            for (var i = 1; i <= intersectingNumbers.Count; i++)
            {
                copyCardsIds.Add(cardId + i);
            }

            return new Card
            {
                Id = cardId,
                WinningNumbers = winningNumbers,
                CardNumbers = cardNumbers,
                CopyCardsIds = copyCardsIds,
                Points = GetCardPoints(intersectingNumbers)
            };
        }

        private static int GetCardPoints(IReadOnlyCollection<int> numbers)
        {
            if (!numbers.Any())
                return 0;

            var points = 1;

            for (var i = 0; i < numbers.Count - 1; i++)
            {
                points *= 2;
            }

            return points;
        }

        private static IEnumerable<Card> ProcessCopiesOfCards(List<Card> originalCards)
        {
            foreach (var card in originalCards)
            {
                foreach (var cardToCopyId in card.CopyCardsIds)
                {
                    originalCards.FirstOrDefault(copyCard => copyCard.Id == cardToCopyId)!.NumberOfCopies += card.NumberOfCopies;
                }
            }
            
            return originalCards;
        }

        [GeneratedRegex("\\d+")]
        private static partial Regex DigitRegex();
    }
}

