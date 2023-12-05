using Helpers;

namespace AdventOfCode.Day5
{
    public class SeedRange
    {
        public long SeedRangeStart { get; set; }
        public long SeedRangeEnd { get; set; }
    }

    public class Map
    {
        public MapType MapType { get; set; }
        public List<MapRange> MapRanges { get; set; } = new();
    }
    
    public class MapRange
    {
        public long SourceRangeFirst { get; set; }
        public long SourceRangeLast { get; set; }
        public long SourceToDestinationDifference { get; set; }
    }
    
    public static class Day5
    {
        public static void Main(string[] args)
        {
            var almanac = FilesHelper.ReadPuzzleInputToLines().ToList();
            
            var seeds = GetSeeds(almanac).ToList();
            var maps = MapType.AllGroups.Select(mapType => GetMapForType(mapType, almanac)).ToList();

            // var locations = seeds.Select(seed => GetLocation(seed, maps.ToList())).ToList();
            // Console.WriteLine($"Closest location (from seeds): {locations.Min()}");

            var seedRanges = GetSeedRanges(seeds).ToList();
            var minLocation = GetMinLocation(seedRanges, maps);
            Console.WriteLine($"Closest location (from seeds with ranges): {minLocation}");
        }

        private static long MapSourceToDestination(long source, List<MapRange>? mapRanges)
        {
            if (mapRanges == null)
            {
                return source;
            }

            var destination = source;
            foreach (var mapRange in mapRanges)
            {
                if (mapRange.SourceRangeFirst <= destination && destination <= mapRange.SourceRangeLast)
                {
                    destination += mapRange.SourceToDestinationDifference;
                    break;
                }
            }

            return destination;
        }

        private static long MapDestinationToSource(long destination, List<MapRange>? mapRanges)
        {
            if (mapRanges == null)
            {
                return destination;
            }

            var source = destination;
            foreach (var mapRange in mapRanges)
            {
                var possibleSource = destination - mapRange.SourceToDestinationDifference;
                
                if (mapRange.SourceRangeFirst <= possibleSource && possibleSource <= mapRange.SourceRangeLast)
                {
                    source = possibleSource;
                    break;
                }
            }

            return source;
        }

        private static long GetTransformedValue(long originalValue, IEnumerable<Map> maps, MapType mapType, bool sourceToDestination)
        {
            var mapRanges = maps.FirstOrDefault(map => map.MapType.Equals(mapType))?.MapRanges;
            var transformedValue = 0L;
            if (sourceToDestination)
            {
                transformedValue = MapSourceToDestination(originalValue, mapRanges);
            }
            else
            {
                transformedValue = MapDestinationToSource(originalValue, mapRanges);
            }
            return transformedValue;
        }
        
        private static long GetLocation(long seed, IReadOnlyCollection<Map> maps)
        {
            // seed-to-soil
            var soil = GetTransformedValue(seed, maps, MapType.SeedToSoil, true);

            // soil-to-fertilizer
            var fertilizer = GetTransformedValue(soil, maps, MapType.SoilToFertilizer, true);
            
            // fertilizer-to-water
            var water = GetTransformedValue(fertilizer, maps, MapType.FertilizerToWater, true);
            
            // water-to-light
            var light = GetTransformedValue(water, maps, MapType.WaterToLight, true);
            
            // light-to-temperature
            var temperature = GetTransformedValue(light, maps, MapType.LightToTemperature, true);
            
            // temperature-to-humidity
            var humidity = GetTransformedValue(temperature, maps, MapType.TemperatureToHumidity, true);
            
            // humidity-to-location
            var location = GetTransformedValue(humidity, maps, MapType.HumidityToLocation, true);
            
            // Console.WriteLine($"Seed: {seed.Id} | Soil: {soil} | Fertilizer: {fertilizer} | Water: {water} | Light: {light} | Temperature: {temperature} | Humidity: {humidity} | Location: {location}");
            
            return location;
        }

        private static long GetMinLocation(IReadOnlyCollection<SeedRange> seedRanges, IReadOnlyCollection<Map> maps)
        {
            var minLocation = 0;
            while (!IsLocationPossibleWithExistingSeeds(minLocation, maps, seedRanges))
            {
                minLocation++;
            }

            return minLocation;
        }

        private static bool IsLocationPossibleWithExistingSeeds(long location, IReadOnlyCollection<Map> maps, IEnumerable<SeedRange> seedRanges)
        {
            // location-to-humidity
            var humidity = GetTransformedValue(location, maps, MapType.HumidityToLocation, false);
            
            // temperature-to-humidity
            var temperature = GetTransformedValue(humidity, maps, MapType.TemperatureToHumidity, false);
            
            // light-to-temperature
            var light = GetTransformedValue(temperature, maps, MapType.LightToTemperature, false);
            
            // water-to-light
            var water = GetTransformedValue(light, maps, MapType.WaterToLight, false);
            
            // fertilizer-to-water
            var fertilizer = GetTransformedValue(water, maps, MapType.FertilizerToWater, false);
            
            // soil-to-fertilizer
            var soil = GetTransformedValue(fertilizer, maps, MapType.SoilToFertilizer, false);
            
            // soil-to-fertilizer
            var seed = GetTransformedValue(soil, maps, MapType.SeedToSoil, false);

            return seedRanges.Any(seedRange => seedRange.SeedRangeStart <= seed && seed <= seedRange.SeedRangeEnd);
        }

        private static IEnumerable<long> GetSeeds(IEnumerable<string> almanac)
        {
            var seedList = almanac
                .FirstOrDefault(line => line.Contains("seeds"))
                ?.Split(':', StringSplitOptions.TrimEntries)[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse);
            
            return seedList ?? new List<long>();
        }

        private static IEnumerable<SeedRange> GetSeedRanges(IReadOnlyList<long> seeds)
        {
            var seedRanges = new List<SeedRange>();
            
            for (var i = 0; i < seeds.Count; i += 2)
            {
                seedRanges.Add(new SeedRange
                {
                    SeedRangeStart = seeds[i],
                    SeedRangeEnd = seeds[i] + seeds[i + 1] - 1
                });
            }

            return seedRanges;
        }
        
        private static Map GetMapForType(MapType mapType, IReadOnlyCollection<string> almanac)
        {
            var map = new Map
            {
                MapType = mapType, 
                MapRanges = GetMapRangesForType(mapType, almanac)
            };

            return map;
        }

        private static List<MapRange> GetMapRangesForType(MapType mapType, IReadOnlyCollection<string> almanac)
        {
            var indexOfMap = almanac.ToList().FindIndex(line => line.Contains(mapType));
            var mapLines = almanac
                           .Select((line, i) => new { i, line })
                           .Where(mapItem => mapItem.i > indexOfMap)
                           .TakeWhile(mapItem => !string.IsNullOrWhiteSpace(mapItem.line))
                           .Select(mapItem => mapItem.line)
                           .ToList();

            return mapLines.Select(line =>
            {
                var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                return new MapRange
                {
                    SourceRangeFirst = splitLine[1],
                    SourceRangeLast = splitLine[1] + splitLine[2],
                    SourceToDestinationDifference = splitLine[0] - splitLine[1]
                };
            }).ToList();
        }
    }
}