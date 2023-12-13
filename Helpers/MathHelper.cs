namespace Helpers;

public static class MathHelper
{
    public static long Gcd(long n1, long n2)
    {
        while (true)
        {
            if (n2 == 0) return n1;
            var n3 = n1;
            n1 = n2;
            n2 = n3 % n2;
        }
    }

    public static long Lcm(IEnumerable<long> numbers)
    {
        return numbers.Aggregate((s, val) => s * val / Gcd(s, val));
    }
}