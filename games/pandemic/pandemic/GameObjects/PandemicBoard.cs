using System.Collections.Generic;
using System.Linq;
using ailib.DataStructures;
using pandemic.Data;

namespace pandemic.GameObjects
{
    public class PandemicBoard
    {
        public IList<City> Cities => _cityGraph.Nodes;

        private readonly IGraph<City> _cityGraph;
        private readonly Dictionary<string, City> _cityNameLookup;
        private readonly Dictionary<City, int> _cityIdxLookup;

        private PandemicBoard(IGraph<City> cityGraph)
        {
            _cityGraph = cityGraph;
            _cityNameLookup = BuildCityNameLookup(_cityGraph.Nodes);
            _cityIdxLookup = BuildCityIdxLookup(_cityGraph.Nodes);
        }

        public static PandemicBoard FromGraph(IGraph<City> cityGraph)
        {
            return new PandemicBoard(cityGraph);
        }
        
        public static PandemicBoard FromCitiesAndEdges(IEnumerable<City> cities, IEnumerable<Edge> edges)
        {
            var cityGraph = new UndirectedGraph<City>();
            
            foreach (var city in cities)
            {
                cityGraph.AddNode(city);
            }

            foreach (var edge in edges)
            {
                cityGraph.AddEdge(edge.From, edge.To);
            }
            
            return new PandemicBoard(cityGraph);
        }

        public static PandemicBoard CreateRealGameBoard()
        {
            return new PandemicBoard(BoardData.LoadCityGraph());
        }

        public City GetCity(string name)
        {
            return _cityNameLookup[name];
        }

        public List<City> Adjacent(City city)
        {
            var cityIdx = _cityIdxLookup[city];
            return _cityGraph.Adjacent(cityIdx).ToList();
        }

        private static Dictionary<string, City> BuildCityNameLookup(IEnumerable<City> cities)
        {
            return cities.ToDictionary(c => c.Name, c => c);
        }
        
        private static Dictionary<City, int> BuildCityIdxLookup(IEnumerable<City> cities)
        {
            var cityList = cities.ToList();
            var lookup = new Dictionary<City, int>();
      
            for (var i = 0; i < cityList.Count; i++)
            {
                lookup[cityList[i]] = i;
            }

            return lookup;
        }
    }
}