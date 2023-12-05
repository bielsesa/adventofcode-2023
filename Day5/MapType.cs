namespace AdventOfCode.Day5;

public struct MapType
{
    public static readonly MapType SeedToSoil = new("seed-to-soil");
    public static readonly MapType SoilToFertilizer = new("soil-to-fertilizer");
    public static readonly MapType FertilizerToWater = new("fertilizer-to-water");
    public static readonly MapType WaterToLight = new("water-to-light");
    public static readonly MapType LightToTemperature = new("light-to-temperature");
    public static readonly MapType TemperatureToHumidity = new("temperature-to-humidity");
    public static readonly MapType HumidityToLocation = new("humidity-to-location");
    public static readonly MapType Invalid = new("N/A");

    public static IEnumerable<MapType> AllGroups
    {
        get
        {
            yield return SeedToSoil;
            yield return SoilToFertilizer;
            yield return FertilizerToWater;
            yield return WaterToLight;
            yield return LightToTemperature;
            yield return TemperatureToHumidity;
            yield return HumidityToLocation;
        }
    }

    private string value;

    /// <summary>
    /// default constructor
    /// </summary>
    public MapType()
    {
        value = "N/A";
    }
    
    /// <summary>
    /// primary constructor
    /// </summary>
    /// <param name="value">The string value that this is a wrapper for</param>
    private MapType(string value)
    {
        this.value = value;
    }

    /// <summary>
    /// Compares the Group to another group, or to a string value.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is MapType)
        {
            return this.value.Equals(((MapType)obj).value);
        }

        string otherString = obj as string;
        if (otherString != null)
        {
            return this.value.Equals(otherString);
        }

        throw new ArgumentException("obj is neither a Group nor a String");
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    /// <summary>
    /// returns the internal string that this is a wrapper for.
    /// </summary>
    /// <param name="mapType</param>
    /// <returns></returns>
    public static implicit operator string(MapType mapType)
    {
        return mapType.value;
    }

    /// <summary>
    /// Parses a string and returns an instance that corresponds to it.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static MapType Parse(string input)
    {
        return AllGroups.Where(item => item.value == input).FirstOrDefault();
    }

    /// <summary>
    /// Syntatic sugar for the Parse method.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public static explicit operator MapType(string other)
    {
        return Parse(other);
    }

    public override string ToString()
    {
        return value;
    }
}