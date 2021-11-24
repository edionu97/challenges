using Newtonsoft.Json;
using SetiEncoderDecoder.Mappings.Constants;
using SetiEncoderDecoder.Mappings.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SetiEncoderDecoder.Mappings.Mapper.Impl
{
    public class WebBasedChemicalElementMapper : IChemicalElementMapper
    {
        private IDictionary<int, string> _atomicNumberToSymbolMappings;
        private IDictionary<string, int> _symbolToAtomicNumberMappings;

        private WebBasedChemicalElementMapper()
        {
        }

        public static async Task<IChemicalElementMapper> CreateAsync()
        {
            //create a new mapper
            WebBasedChemicalElementMapper mapper = new();

            //load all mappings
            await mapper.LoadAllMappingsAsync();

            //return mapper
            return mapper;
        }


        public string this[int atomicNumber]
        {
            get
            {
                //try get value
                _atomicNumberToSymbolMappings.TryGetValue(atomicNumber, out var mapping);

                //return mapping
                return mapping;
            }
        }

        public int this[string symbol]
        {
            get
            {
                //try get value
                _symbolToAtomicNumberMappings.TryGetValue(symbol, out var mapping);

                //return mapping
                return mapping;
            }
        }

        public ICollection<string> Symbols => _symbolToAtomicNumberMappings.Keys;

        public async Task LoadAllMappingsAsync()
        {
            //load all data
            var allData = 
                (await LoadAllDataAsync()).ToList();

            //create mappings for number symbol
            _atomicNumberToSymbolMappings = allData
                .ToDictionary(x => x.AtomicNumber, x => x.Symbol);

            //create symbol for attomig mappings
            _symbolToAtomicNumberMappings = allData
                .ToDictionary(x => x.Symbol, x => x.AtomicNumber);
        }

        private static async Task<IEnumerable<ChemicalElement>> LoadAllDataAsync()
        {
            //create a new http client
            using HttpClient client = new();

            //get the response
            var response = await client.GetAsync(ApiConstants.GetAllElementsApi);

            //read content
            var content = await response.Content.ReadAsStringAsync();

            //deserialize object list
            return JsonConvert.DeserializeObject<List<ChemicalElement>>(content);
        }
    }
}
