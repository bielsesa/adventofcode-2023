using Helpers;

namespace AdventOfCode.Day5
{
    public class Seed
    {
        public long Id { get; set; }
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

            var locations = seeds.Select(seed => GetLocation(seed, maps.ToList())).ToList();
            Console.WriteLine($"Closest location: {locations.Min()}");
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
        
        private static long GetLocation(Seed seed, IReadOnlyCollection<Map> maps)
        {
            // seed-to-soil
            var seedToSoilRanges = maps.FirstOrDefault(map => map.MapType.Equals(MapType.SeedToSoil))?.MapRanges;
            var soil = MapSourceToDestination(seed.Id, seedToSoilRanges);

            // soil-to-fertilizer
            var soilToFertilizer = maps.FirstOrDefault(map => map.MapType.Equals(MapType.SoilToFertilizer))?.MapRanges;
            var fertilizer = MapSourceToDestination(soil, soilToFertilizer);
            
            // fertilizer-to-water
            var fertilizerToWater = maps.FirstOrDefault(map => map.MapType.Equals(MapType.FertilizerToWater))?.MapRanges;
            var water = MapSourceToDestination(fertilizer, fertilizerToWater);
            
            // water-to-light
            var waterToLight = maps.FirstOrDefault(map => map.MapType.Equals(MapType.WaterToLight))?.MapRanges;
            var light = MapSourceToDestination(water, waterToLight);
            
            // light-to-temperature
            var lightToTemperature = maps.FirstOrDefault(map => map.MapType.Equals(MapType.LightToTemperature))?.MapRanges;
            var temperature = MapSourceToDestination(light, lightToTemperature);
            
            // temperature-to-humidity
            var temperatureToHumidity = maps.FirstOrDefault(map => map.MapType.Equals(MapType.TemperatureToHumidity))?.MapRanges;
            var humidity = MapSourceToDestination(temperature, temperatureToHumidity);
            
            // humidity-to-location
            var humidityToLocation = maps.FirstOrDefault(map => map.MapType.Equals(MapType.HumidityToLocation))?.MapRanges;
            var location = MapSourceToDestination(humidity, humidityToLocation);
            
            // Console.WriteLine($"Seed: {seed.Id} | Soil: {soil} | Fertilizer: {fertilizer} | Water: {water} | Light: {light} | Temperature: {temperature} | Humidity: {humidity} | Location: {location}");
            
            return location;
        }

        private static IEnumerable<Seed> GetSeeds(IEnumerable<string> almanac)
        {
            var seedList = almanac
                .FirstOrDefault(line => line.Contains("seeds"))
                ?.Split(':', StringSplitOptions.TrimEntries)[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            return seedList != null 
                ? seedList.Select(seedId => new Seed{Id = long.Parse(seedId)})
                : new List<Seed>();
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