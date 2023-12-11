namespace AdventOfCode.Day6;

public class Hand : IComparable
{
    public List<Card> Cards { get; set; }
    public List<Card> CardsTransformed { get; set; }
    public Card FirstCard => Cards[0];
    public Card SecondCard => Cards[1];
    public Card ThirdCard => Cards[2];
    public Card FourthCard => Cards[3];
    public Card FifthCard => Cards[4];
    public long Bid { get; set; }
    public HandType HandType { get; set; }

    public static bool operator == (Hand a, Hand b)
    {
        return a.IsEqualRank(b);
    }

    public static bool operator != (Hand a, Hand b)
    {
        return !(a == b);
    }

    public static bool operator <= (Hand a, Hand b)
    {
        return a.IsEqualRank(b) || a.IsLowerRank(b);
    }
    
    public static bool operator >= (Hand a, Hand b)
    {
        return a.IsEqualRank(b) || a.IsHigherRank(b);
    }
    
    public static bool operator < (Hand a, Hand b)
    {
        return a.IsLowerRank(b);
    }

    public static bool operator > (Hand a, Hand b)
    {
        return a.IsHigherRank(b);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
               {
                   Hand hand => this.IsEqualRank(hand),
                   _ => throw new ArgumentException($"obj is not of type {typeof(Hand)}")
               };
    }

    public override string ToString()
    {
        return $"{FirstCard}{SecondCard}{ThirdCard}{FourthCard}{FifthCard} {Bid}";
    }

    public int CompareTo(object? obj)
    {
        if (obj is Hand otherHand)
        {
            if (this < otherHand)
            {
                return -1;
            }
            
            if (this > otherHand)
            {
                return 1;
            }
            
            if (this == otherHand && this.Bid < otherHand.Bid)
            {
                return -1;
            }

            return 0;
        }

        throw new ArgumentException($"obj is not of type {typeof(Hand)}");
    }

    /// <summary>
    /// Creates a hand from a string formatted in a specific way.
    /// </summary>
    /// <param name="handAndBidLine">The string containing the hand and the bid.</param>
    /// <returns>A <c>Hand</c> with the information extracted from a string.</returns>
    public static Hand ExtractHandFromString(string handAndBidLine)
    {
        var handAndBid = handAndBidLine.Split();
        var cards = handAndBid[0].ToCharArray().Select(cardString => Card.Parse(cardString.ToString())).ToList();
        var (handType, cardsTransformed) = HandType.GetHandTypeAndTransformedCards(cards);
        
        return new Hand
        {
            Cards = cards,
            CardsTransformed = cardsTransformed,
            Bid = long.Parse(handAndBid[1]),
            HandType = handType
        };
    }
}

public static class HandExtensions
{
    public static bool IsEqualRank(this Hand a, Hand b)
    {
        return a.HandType == b.HandType;
    }

    public static bool IsLowerRank(this Hand a, Hand b)
    {
        return !a.IsHigherRank(b);
    }
    
    public static bool IsHigherRank(this Hand a, Hand b)
    {
        if (!a.HandType.Equals(b.HandType)) 
            return a.HandType > b.HandType;
        
        foreach (var card in a.Cards.Select((value, i) => new { i, value }))
        {
            if (card.value > b.Cards[card.i])
                return true;
            if (b.Cards[card.i] > card.value)
                return false;
        }

        return a.HandType > b.HandType;
    }
}