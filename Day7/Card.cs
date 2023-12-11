namespace AdventOfCode.Day6;

public struct Card
{
    private const string cardPriority = "J23456789TQKA";
 
    public static readonly Card A = new("A");
    public static readonly Card K = new("K");
    public static readonly Card Q = new("Q");
    public static readonly Card J = new("J");
    public static readonly Card T = new("T");
    public static readonly Card Nine = new("9");
    public static readonly Card Eight = new("8");
    public static readonly Card Seven = new("7");
    public static readonly Card Six = new("6");
    public static readonly Card Five = new("5");
    public static readonly Card Four = new("4");
    public static readonly Card Three = new("3");
    public static readonly Card Two = new("2");

    public static IEnumerable<Card> AllGroups
    {
        get
        {
            yield return A;
            yield return K;
            yield return Q;
            yield return J;
            yield return T;
            yield return Nine;
            yield return Eight;
            yield return Seven;
            yield return Six;
            yield return Five;
            yield return Four;
            yield return Three;
            yield return Two;
        }
    }

    private string value;
    
    /// <summary>
    /// default constructor
    /// </summary>
    /// <param name="value">The string value that this is a wrapper for</param>
    private Card(string value)
    {
        this.value = value;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="card">The card to copy</param>
    public Card(Card card)
    {
        this.value = card.value;
    }

    public static bool operator <=(Card a, Card b)
    {
        return a.Equals(b) || a < b;
    }

    public static bool operator >=(Card a, Card b)
    {
        return a.Equals(b) || a > b;
    }

    public static bool operator <(Card a, Card b)
    {
        return !(a > b);
    }

    public static bool operator >(Card a, Card b)
    {
        return cardPriority.IndexOf(a) > cardPriority.IndexOf(b);
    }

    /// <summary>
    /// Compares the Group to another group, or to a string value.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj switch
               {
                   Card card => value.Equals(card.value),
                   string otherString => value.Equals(otherString),
                   _ => throw new ArgumentException("obj is neither a Card nor a String")
               };
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    /// <summary>
    /// Returns the internal string that this is a wrapper for.
    /// </summary>
    /// <param name="card">The card.</param>
    /// <returns></returns>
    public static implicit operator string(Card card)
    {
        return card.value;
    }

    /// <summary>
    /// Parses a string and returns an instance that corresponds to it.
    /// </summary>
    /// <param name="input">The string value of the card.</param>
    /// <returns></returns>
    public static Card Parse(string input)
    {
        return AllGroups.FirstOrDefault(item => item.value == input);
    }

    /// <summary>
    /// Syntactic sugar for the Parse method.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public static explicit operator Card(string other)
    {
        return Parse(other);
    }

    public override string ToString()
    {
        return value;
    }
}