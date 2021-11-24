using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetiEncoderDecoder.Decoder.Decoding.Impl
{
    public partial class SetiDecoder
    {
        private IEnumerable<string> CreatePriorityList()
        {
            //get the symbols
            var remainingItems = _elementMapper
                .Symbols
                .Where(s => s != "C" && s != "H")
                .OrderBy(s => s);

            //return the priority list
            return new[] { "C", "H" }.Concat(remainingItems);
        }

        private string BuildDecodedSeq(IDictionary<string, int> messageElements)
        {
            var builder = new StringBuilder();

            //iterate the symbol
            foreach (var symbol in _hillSystemList)
            {
                //remove the keys that are not in set
                if (!messageElements.ContainsKey(symbol))
                {
                    continue;
                }

                if (messageElements[symbol] != 1)
                {

                }

                //get symbol count
                var symbolCount = messageElements[symbol] > 1 
                    ? messageElements[symbol].ToString() 
                    : string.Empty ;

                //create the decoded string
                builder.Append($"{symbol}{symbolCount}");
            }

            return builder.ToString();
        }
    }
}
