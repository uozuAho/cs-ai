using System.Collections.Generic;
using ailib.DataStructures;
using pandemic.GameObjects;

namespace pandemic.Data
{
  public static class BoardData
  {
    public static UndirectedGraph<City> LoadCityGraph()
    {
      var graph = new UndirectedGraph<City>();
      var idxLookup = BuildCityIdxLookup(Cities);

      foreach (var city in Cities) graph.AddNode(city);
      
      foreach (var (cityFrom, cityTo) in Edges)
      {
        graph.AddEdge(idxLookup[cityFrom], idxLookup[cityTo]);
      }

      return graph;
    }

    private static Dictionary<string, int> BuildCityIdxLookup(City[] cities)
    {
      var lookup = new Dictionary<string, int>();
      
      for (var i = 0; i < cities.Length; i++)
      {
        lookup[cities[i].Name] = i;
      }

      return lookup;
    }
    
    private static readonly City[] Cities = new[]
    {
      new City
      {
        Name = "San Francisco",
        Colour = Colour.Blue,
        Location = new Point2D(-959.0986, 974.0585),
      },
      new City
      {
        Name = "Chicago",
        Colour = Colour.Blue,
        Location = new Point2D(-826.1213, 1027.2494),
      },
      new City
      {
        Name = "Montreal",
        Colour = Colour.Blue,
        Location = new Point2D(-719.7395, 1030.5739),
      },
      new City
      {
        Name = "New York",
        Colour = Colour.Blue,
        Location = new Point2D(-586.7622, 990.6806),
      },
      new City
      {
        Name = "Washington",
        Colour = Colour.Blue,
        Location = new Point2D(-596.7355, 827.78345),
      },
      new City
      {
        Name = "Atlanta",
        Colour = Colour.Blue,
        Location = new Point2D(-789.55255, 824.45905),
      },
      new City
      {
        Name = "Madrid",
        Colour = Colour.Blue,
        Location = new Point2D(-407.2429, 847.73004),
      },
      new City
      {
        Name = "London",
        Colour = Colour.Blue,
        Location = new Point2D(-410.56732, 1007.3028),
      },
      new City
      {
        Name = "Paris",
        Colour = Colour.Blue,
        Location = new Point2D(-224.39914, 917.5431),
      },
      new City
      {
        Name = "Essen",
        Colour = Colour.Blue,
        Location = new Point2D(-207.77698, 1067.1426),
      },
      new City
      {
        Name = "Milan",
        Colour = Colour.Blue,
        Location = new Point2D(-78.124146, 967.4096),
      },
      new City
      {
        Name = "St. Petersburg",
        Colour = Colour.Blue,
        Location = new Point2D(18.284374, 1057.1692),
      },
      new City
      {
        Name = "Algiers",
        Colour = Colour.Black,
        Location = new Point2D(-71.63187, 796.3551),
      },
      new City
      {
        Name = "Istanbul",
        Colour = Colour.Black,
        Location = new Point2D(74.824005, 911.7446),
      },
      new City
      {
        Name = "Moscow",
        Colour = Colour.Black,
        Location = new Point2D(199.0896, 1038.2292),
      },
      new City
      {
        Name = "Cairo",
        Colour = Colour.Black,
        Location = new Point2D(130.26338, 736.04895),
      },
      new City
      {
        Name = "Baghdad",
        Colour = Colour.Black,
        Location = new Point2D(249.61157, 849.00354),
      },
      new City
      {
        Name = "Tehran",
        Colour = Colour.Black,
        Location = new Point2D(381.7471, 996.05756),
      },
      new City
      {
        Name = "Delhi",
        Colour = Colour.Black,
        Location = new Point2D(541.58844, 883.10297),
      },
      new City
      {
        Name = "Karachi",
        Colour = Colour.Black,
        Location = new Point2D(473.38947, 793.5918),
      },
      new City
      {
        Name = "Riyadh",
        Colour = Colour.Black,
        Location = new Point2D(362.56613, 659.32513),
      },
      new City
      {
        Name = "Mumbai",
        Colour = Colour.Black,
        Location = new Point2D(537.326, 522.9272),
      },
      new City
      {
        Name = "Chennai",
        Colour = Colour.Black,
        Location = new Point2D(639.62445, 424.89117),
      },
      new City
      {
        Name = "Kolkata",
        Colour = Colour.Black,
        Location = new Point2D(633.23083, 744.57385),
      },
      new City
      {
        Name = "Beijing",
        Colour = Colour.Red,
        Location = new Point2D(729.73303, 978.17725),
      },
      new City
      {
        Name = "Seoul",
        Colour = Colour.Red,
        Location = new Point2D(943.52686, 968.2334),
      },
      new City
      {
        Name = "Tokyo",
        Colour = Colour.Red,
        Location = new Point2D(1178.8657, 921.8285),
      },
      new City
      {
        Name = "Shanghai",
        Colour = Colour.Red,
        Location = new Point2D(769.50867, 830.67615),
      },
      new City
      {
        Name = "Hong Kong",
        Colour = Colour.Red,
        Location = new Point2D(781.10986, 722.95056),
      },
      new City
      {
        Name = "Taipei",
        Colour = Colour.Red,
        Location = new Point2D(1013.13416, 772.67004),
      },
      new City
      {
        Name = "Osaka",
        Colour = Colour.Red,
        Location = new Point2D(1155.6633, 824.0469),
      },
      new City
      {
        Name = "Bangkok",
        Colour = Colour.Red,
        Location = new Point2D(796.0257, 583.73596),
      },
      new City
      {
        Name = "Ho Chi Minh City",
        Colour = Colour.Red,
        Location = new Point2D(1024.7354, 538.98846),
      },
      new City
      {
        Name = "Manila",
        Colour = Colour.Red,
        Location = new Point2D(1197.0962, 583.73596),
      },
      new City
      {
        Name = "Jakarta",
        Colour = Colour.Red,
        Location = new Point2D(888.83545, 401.4312),
      },
      new City
      {
        Name = "Sydney",
        Colour = Colour.Red,
        Location = new Point2D(1177.2085, 131.28864),
      },
      new City
      {
        Name = "Khartoum",
        Colour = Colour.Yellow,
        Location = new Point2D(196.62912, 577.29913),
      },
      new City
      {
        Name = "Johannesburg",
        Colour = Colour.Yellow,
        Location = new Point2D(108.15817, 176.57779),
      },
      new City
      {
        Name = "Kinshasa",
        Colour = Colour.Yellow,
        Location = new Point2D(-79.19205, 395.15308),
      },
      new City
      {
        Name = "Lagos",
        Colour = Colour.Yellow,
        Location = new Point2D(-178.07135, 556.4824),
      },
      new City
      {
        Name = "Sao Paulo",
        Colour = Colour.Yellow,
        Location = new Point2D(-456.49463, 335.30508),
      },
      new City
      {
        Name = "Buenos Aires",
        Colour = Colour.Yellow,
        Location = new Point2D(-583.9969, 64.68808),
      },
      new City
      {
        Name = "Santiago",
        Colour = Colour.Yellow,
        Location = new Point2D(-919.666, 7.442169),
      },
      new City
      {
        Name = "Lima",
        Colour = Colour.Yellow,
        Location = new Point2D(-979.51404, 265.04874),
      },
      new City
      {
        Name = "Bogota",
        Colour = Colour.Yellow,
        Location = new Point2D(-771.3471, 405.5614),
      },
      new City
      {
        Name = "Mexico City",
        Colour = Colour.Yellow,
        Location = new Point2D(-961.29944, 572.095),
      },
      new City
      {
        Name = "Los Angeles",
        Colour = Colour.Yellow,
        Location = new Point2D(-992.5244, 793.2723),
      },
      new City
      {
        Name = "Miami",
        Colour = Colour.Yellow,
        Location = new Point2D(-706.2949, 650.15753),
      }
    };

    private static readonly List<(string, string)> Edges = new List<(string, string)>
    {
      ( "San Francisco", "Chicago" ),
      ( "San Francisco", "Tokyo" ),
      ( "Chicago", "Montreal" ),
      ( "Chicago", "Atlanta" ),
      ( "New York", "Montreal" ),
      ( "New York", "Washington" ),
      ( "New York", "Madrid" ),
      ( "New York", "London" ),
      ( "Washington", "Montreal" ),
      ( "Washington", "Atlanta" ),
      ( "Washington", "Miami" ),
      ( "Madrid", "London" ),
      ( "Madrid", "Paris" ),
      ( "London", "Paris" ),
      ( "Essen", "London" ),
      ( "Essen", "Paris" ),
      ( "Essen", "Milan" ),
      ( "Essen", "St. Petersburg" ),
      ( "Milan", "Paris" ),
      ( "St. Petersburg", "Moscow" ),
      ( "Algiers", "Madrid" ),
      ( "Algiers", "Paris" ),
      ( "Algiers", "Istanbul" ),
      ( "Algiers", "Cairo" ),
      ( "Istanbul", "Milan" ),
      ( "Istanbul", "St. Petersburg" ),
      ( "Moscow", "Istanbul" ),
      ( "Cairo", "Istanbul" ),
      ( "Cairo", "Baghdad" ),
      ( "Cairo", "Khartoum" ),
      ( "Baghdad", "Istanbul" ),
      ( "Baghdad", "Tehran" ),
      ( "Tehran", "Moscow" ),
      ( "Tehran", "Delhi" ),
      ( "Tehran", "Karachi" ),
      ( "Delhi", "Chennai" ),
      ( "Delhi", "Kolkata" ),
      ( "Karachi", "Baghdad" ),
      ( "Karachi", "Delhi" ),
      ( "Karachi", "Mumbai" ),
      ( "Riyadh", "Cairo" ),
      ( "Riyadh", "Baghdad" ),
      ( "Riyadh", "Karachi" ),
      ( "Mumbai", "Delhi" ),
      ( "Mumbai", "Chennai" ),
      ( "Chennai", "Kolkata" ),
      ( "Chennai", "Bangkok" ),
      ( "Chennai", "Jakarta" ),
      ( "Kolkata", "Hong Kong" ),
      ( "Kolkata", "Bangkok" ),
      ( "Beijing", "Seoul" ),
      ( "Seoul", "Tokyo" ),
      ( "Tokyo", "Shanghai" ),
      ( "Shanghai", "Beijing" ),
      ( "Shanghai", "Seoul" ),
      ( "Shanghai", "Taipei" ),
      ( "Hong Kong", "Shanghai" ),
      ( "Hong Kong", "Taipei" ),
      ( "Taipei", "Osaka" ),
      ( "Osaka", "Tokyo" ),
      ( "Bangkok", "Hong Kong" ),
      ( "Bangkok", "Jakarta" ),
      ( "Ho Chi Minh City", "Hong Kong" ),
      ( "Ho Chi Minh City", "Bangkok" ),
      ( "Manila", "San Francisco" ),
      ( "Manila", "Hong Kong" ),
      ( "Manila", "Taipei" ),
      ( "Manila", "Ho Chi Minh City" ),
      ( "Jakarta", "Ho Chi Minh City" ),
      ( "Jakarta", "Sydney" ),
      ( "Sydney", "Manila" ),
      ( "Khartoum", "Johannesburg" ),
      ( "Johannesburg", "Kinshasa" ),
      ( "Kinshasa", "Khartoum" ),
      ( "Kinshasa", "Lagos" ),
      ( "Lagos", "Khartoum" ),
      ( "Sao Paulo", "Madrid" ),
      ( "Sao Paulo", "Lagos" ),
      ( "Sao Paulo", "Buenos Aires" ),
      ( "Sao Paulo", "Bogota" ),
      ( "Buenos Aires", "Bogota" ),
      ( "Lima", "Santiago" ),
      ( "Lima", "Mexico City" ),
      ( "Bogota", "Lima" ),
      ( "Mexico City", "Chicago" ),
      ( "Mexico City", "Bogota" ),
      ( "Mexico City", "Los Angeles" ),
      ( "Mexico City", "Miami" ),
      ( "Los Angeles", "San Francisco" ),
      ( "Los Angeles", "Chicago" ),
      ( "Los Angeles", "Sydney" ),
      ( "Miami", "Atlanta" ),
      ( "Miami", "Bogota" ),
    };
  }
}