using System.Runtime.CompilerServices;
using Helpers;

namespace AdventOfCode.Day6;

public struct HandType
{
    public static readonly HandType FiveOfAKind = new("Five of a kind");
    public static readonly HandType FourOfAKind = new("Four of a kind");
    public static readonly HandType FullHouse = new("Full house");
    public static readonly HandType ThreeOfAKind = new("Three of a kind");
    public static readonly HandType TwoPair = new("Two pair");
    public static readonly HandType OnePair = new("One pair");
    public static readonly HandType HighCard = new("High card");
    
    private string value;
    
    public static IEnumerable<HandType> AllGroups
    {
        get
        {
            yield return FiveOfAKind;
            yield return FourOfAKind;
            yield return FullHouse;
            yield return ThreeOfAKind;
            yield return TwoPair;
            yield return OnePair;
            yield return HighCard;
        }
    }
    
    private HandType(string value)
    {
        this.value = value;
    }

    public static bool operator ==(HandType a, HandType b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(HandType a, HandType b)
    {
        return !(a == b);
    }

    public static bool operator <=(HandType a, HandType b)
    {
        return a.Equals(b) || a < b;
    }

    public static bool operator >=(HandType a, HandType b)
    {
        return a.Equals(b) || a > b;
    }

    public static bool operator <(HandType a, HandType b)
    {
        return !(a > b);
    }

    public static bool operator >(HandType a, HandType b)
    {
        if (a.Equals(FiveOfAKind) && !b.Equals(FiveOfAKind))
        {
            return true;
        }
        
        if (a.Equals(FourOfAKind) && !b.Equals(FiveOfAKind) && !b.Equals(FourOfAKind))
        {
            return true;
        }
        
        if (a.Equals(FullHouse) && !b.Equals(FiveOfAKind) && !b.Equals(FourOfAKind) && !b.Equals(FullHouse))
        {
            return true;
        }
        
        if (a.Equals(ThreeOfAKind) && !b.Equals(FiveOfAKind) && !b.Equals(FourOfAKind) && !b.Equals(FullHouse) && !b.Equals(ThreeOfAKind))
        {
            return true;
        }
        
        if (a.Equals(TwoPair) && !b.Equals(FiveOfAKind) && !b.Equals(FourOfAKind) && !b.Equals(FullHouse) && !b.Equals(ThreeOfAKind) && !b.Equals(TwoPair))
        {
            return true;
        }
        
        if (a.Equals(OnePair) && !b.Equals(FiveOfAKind) && !b.Equals(FourOfAKind) && !b.Equals(FullHouse) && !b.Equals(ThreeOfAKind) && !b.Equals(TwoPair) && !b.Equals(OnePair))
        {
            return true;
        }

        return false;
    }
    
    public override bool Equals(object? obj)
    {
        return obj switch
               {
                   HandType handType => this.value.Equals(handType.value),
                   string otherString => this.value.Equals(otherString),
                   _ => throw new ArgumentException("obj is neither a HandType nor a String")
               };
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
    
    public static implicit operator string(HandType card)
    {
        return card.value;
    }

    public static HandType Parse(string input)
    {
        return AllGroups.FirstOrDefault(item => item.value == input);
    }

    public static explicit operator HandType(string other)
    {
        return Parse(other);
    }

    public override string ToString()
    {
        return value;
    }

    // transform jokers to card with the most repetition
    public static (HandType handType, List<Card> cardsTransformed) GetHandTypeAndTransformedCards(
        List<Card> originalCards)
    {
        if (originalCards.All(card => card.Equals(Card.J)))
        {
            return (FiveOfAKind, originalCards);
        }
        
        List<Card> cardsTransformed;
        var cardsWithoutJoker = new List<Card>(originalCards.Count);

        originalCards.ForEach(card =>
        {
            if (!card.Equals(Card.J)) cardsWithoutJoker.Add(card);
        });
        
        if (originalCards.Any(card => card.Equals(Card.J)))
        {
            cardsTransformed = originalCards.Select(card => card.Equals(Card.J) 
                                                        ? EnumerableHelper<Card>.GetMostRepeatedItem(cardsWithoutJoker)
                                                        : card).ToList();
        }
        else
        {
            cardsTransformed = originalCards;
        }

        return (GetHandTypeFromCards(cardsTransformed), cardsTransformed);
    }

    public static HandType GetHandTypeFromCards(List<Card> cards)
    {
        if (cards.All(c => c == cards[0]))
        {
            return FiveOfAKind;
        }

        if (cards.Distinct().Count() == 5)
        {
            return HighCard;
        }
        
        var groupsOfCards = cards.GroupBy(c => c).ToList();

        if (groupsOfCards.Count() == 2)
        {
            // FourOfAKind
            if (groupsOfCards.Any(group => group.Count() == 4))
            {
                return FourOfAKind;
            }
            
            // FullHouse
            if (groupsOfCards.Any(group => group.Count() == 3))
            {
                return FullHouse;
            }
        }

        if (groupsOfCards.Count() == 3)
        {
            // ThreeOfAKind
            if (groupsOfCards.Any(group => group.Count() == 3))
            {
                return ThreeOfAKind;
            }
            
            // TwoPair
            if (groupsOfCards.Count(group => group.Count() == 2) == 2)
            {
                return TwoPair;
            }
        }

        return OnePair;
    }
}