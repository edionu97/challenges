using System;
using SetiEncoderDecoder.Mappings.Mapper;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace SetiEncoderDecoder.Encoder.Encoding.Impl
{
    public class SetiEncoder : IEncoder
    {
        private readonly Regex _elementIdentificationRegex;
        private readonly IChemicalElementMapper _elementMapper;

        public SetiEncoder(IChemicalElementMapper elementMapper)
        {
            _elementMapper = elementMapper;
            _elementIdentificationRegex = BuildElementIdentificationRegex();
        }

        public string EncodeSequence(string sequence)
        {
            //get matched
            var matches = 
                _elementIdentificationRegex.Matches(sequence);

            //get the numeric identifier
            var numericIdentifier = 
                new Regex("(?<element>[a-zA-Z]+)(?<vallence>[0-9]*)");

            //create the string builder
            var builder = new StringBuilder();

            //iterate matches
            foreach (Match match in matches)
            {
                //get the match value
                var matchValue = match.Value;

                //get the groups
                var elementMatch = 
                    numericIdentifier.Match(matchValue).Groups;

                //parse values
                var elementName = elementMatch["element"].Value;
                _ = int.TryParse(elementMatch["vallence"].Value, out var valence);
                valence = Math.Max(1, valence);

                //get the atomic number
                var setiSignalMap = BuildSeq(_elementMapper[elementName]);

                //repeat the sequence as many times as required
                var encodedSetiSymbol = string
                    .Join("", Enumerable.Repeat(setiSignalMap, valence));

                //append the symbol
                builder.Append(encodedSetiSymbol);
            }

            //return the builder
            return builder.ToString();
        }

        /// <summary>
        /// This method should be used for building the sequence
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string BuildSeq(int number)
        {
            //special case
            if (number == 1)
            {
                return "+-";
            }

            //create the list
            var seqList = Enumerable
                .Repeat("+", number)
                .Concat(Enumerable.Repeat("0", number))
                .Concat(Enumerable.Repeat("-", number));

            return string.Join("", seqList);
        }

        private Regex BuildElementIdentificationRegex()
        {
            //get items
            //build regex in the inverse order of length (bigger element names should be in front)
            var items = _elementMapper
                .Symbols
                .OrderByDescending(x => x.Length)
                .Select(e => $"{e}[0-9]*")
                .ToList();

            //create the regex
            var regex = string.Join('|', items);

            //return new regex
            return new Regex(regex);
        }
    }
}
