namespace AdventOfCode.Day6;

public struct Card
{
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
        if (a.Equals(A) && !b.Equals(A))
            return true;
        if (a.Equals(K) && !b.Equals(A) && !b.Equals(K))
            return true;
        if (a.Equals(Q) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q))
            return true;
        if (a.Equals(J) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J))
            return true;
        if (a.Equals(T) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T))
            return true;
        if (a.Equals(Nine) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) && !b.Equals(Nine))
            return true;
        if (a.Equals(Eight) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight))
            return true;
        if (a.Equals(Seven) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven))
            return true;
        if (a.Equals(Six) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven) && !b.Equals(Six))
            return true;
        if (a.Equals(Five) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven) && !b.Equals(Six) && !b.Equals(Five))
            return true;
        if (a.Equals(Four) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven) && !b.Equals(Six) && !b.Equals(Five)
            && !b.Equals(Four))
            return true;
        if (a.Equals(Three) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
            && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven) && !b.Equals(Six) && !b.Equals(Five)
            && !b.Equals(Four) && !b.Equals(Three))
            return true;
        
        return a.Equals(Two) && !b.Equals(A) && !b.Equals(K) && !b.Equals(Q) && !b.Equals(J) && !b.Equals(T) 
               && !b.Equals(Nine) && !b.Equals(Eight) && !b.Equals(Seven) && !b.Equals(Six) && !b.Equals(Five)
               && !b.Equals(Four) && !b.Equals(Two);
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