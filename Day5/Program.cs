using Helpers;

namespace AdventOfCode.Day5
{
    public class Seed
    {
        public long Id { get; set; }
    }

    public class Map
    {
        public Dictionary<long, long> TotalRange { get; set; } = new();
        public MapType MapType { get; set; }
    }
    
    public class MapRange
    {
        public long DestinationOrigin { get; set; }
        public long SourceOrigin { get; set; }
        public long Step { get; set; }
        public Dictionary<long, long> Range { get; set; } = new();
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

        private static long GetLocation(Seed seed, IReadOnlyCollection<Map> maps)
        {
            // seed-to-soil
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.SeedToSoil))!.TotalRange
                     .TryGetValue(seed.Id, out var soil))
            {
                soil = seed.Id;
            }
            
            // soil-to-fertilizer
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.SoilToFertilizer))!.TotalRange
                     .TryGetValue(soil, out var fertilizer))
            {
                fertilizer = soil;
            }
            
            // fertilizer-to-water
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.FertilizerToWater))!.TotalRange
                     .TryGetValue(fertilizer, out var water))
            {
                water = fertilizer;
            }
            
            // water-to-light
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.WaterToLight))!.TotalRange
                     .TryGetValue(water, out var light))
            {
                light = water;
            }
            
            // light-to-temperature
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.LightToTemperature))!.TotalRange
                     .TryGetValue(light, out var temperature))
            {
                temperature = light;
            }
            
            // temperature-to-humidity
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.TemperatureToHumidity))!.TotalRange
                     .TryGetValue(temperature, out var humidity))
            {
                humidity = temperature;
            }
            
            // humidity-to-location
            if (!maps.FirstOrDefault(map => map.MapType.Equals(MapType.HumidityToLocation))!.TotalRange
                     .TryGetValue(humidity, out var location))
            {
                location = humidity;
            }
            
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
                TotalRange = GetMapRangesForType(mapType, almanac).SelectMany(mapRange => mapRange.Range).ToDictionary(pair => pair.Key, pair => pair.Value)
            };

            return map;
        }

        private static IEnumerable<MapRange> GetMapRangesForType(MapType mapType, IReadOnlyCollection<string> almanac)
        {
            var indexOfMap = almanac.ToList().FindIndex(line => line.Contains(mapType));
            var mapLines = almanac
                           .Select((line, i) => new { i, line })
                           .Where(mapItem => mapItem.i > indexOfMap)
                           .TakeWhile(mapItem => !string.IsNullOrWhiteSpace(mapItem.line))
                           .Select(mapItem => mapItem.line)
                           .ToList();

            var mapRanges = mapLines.Select(line =>
            {
                var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                return new MapRange
                {
                    DestinationOrigin = splitLine[0], 
                    SourceOrigin = splitLine[1], 
                    Step = splitLine[2]
                };
            }).ToList();

            foreach (var mapRange in mapRanges)
            {
                mapRange.Range = GetRangeFromDestinationAndSource(mapRange);
            }

            return mapRanges;
        }

        private static Dictionary<long, long> GetRangeFromDestinationAndSource(MapRange mapRange)
        {
            var range = new Dictionary<long, long>();

            for (var i = 0; i < mapRange.Step; i++)
            {
                range.Add(mapRange.SourceOrigin + i, mapRange.DestinationOrigin + i);
            }
            
            return range;
        }
    }
}