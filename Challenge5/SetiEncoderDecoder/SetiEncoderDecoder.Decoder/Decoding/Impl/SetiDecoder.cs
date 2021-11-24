using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SetiEncoderDecoder.Mappings.Mapper;

namespace SetiEncoderDecoder.Decoder.Decoding.Impl
{
    public partial class SetiDecoder : IDecoder
    {
        private readonly ICollection<string> _hillSystemList;
        private readonly IChemicalElementMapper _elementMapper;

        private readonly Regex _symbolIdentifierRegex 
            = new(@"(?<protons>\++)(?<neutrons>0*)(?<electrons>-+)");

        public SetiDecoder(IChemicalElementMapper elementMapper)
        {
            _elementMapper = elementMapper;
            _hillSystemList = CreatePriorityList().ToList();
        }

        public string DecodeSequence(string sequence)
        {
            //get all the matches
            var matches =
                _symbolIdentifierRegex.Matches(sequence);

            Dictionary<string, int> frequences = new();

            //iterate matches
            foreach(Match match in matches)
            {
                //get the number of protons and electrons and neutrons
                var protons = match.Groups["protons"].Value;
                var neutrons = match.Groups["neutrons"].Value;
                var electrons = match.Groups["electrons"].Value;

                //check protons and electrons length
                if (protons.Length != electrons.Length)
                {
                    throw new Exception("+ and - signs must be equal");
                }

                //check neutrons length
                if (neutrons.Length != 0 && neutrons.Length != protons.Length)
                {
                    throw new Exception("+ - 0 signs must be equal");
                }

                //get the symbol
                var symbol = _elementMapper[protons.Length];

                //increment the number of apparitions
                if (!frequences.ContainsKey(symbol))
                {
                    frequences.Add(symbol, 0);
                }

                //increment the freq
                frequences[symbol] += 1;
            }

            //return the decoded seq
            return BuildDecodedSeq(frequences);
        }
    }
}
