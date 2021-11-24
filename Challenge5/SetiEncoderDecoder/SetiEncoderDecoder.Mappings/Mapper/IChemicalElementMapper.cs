using System.Collections.Generic;
using SetiEncoderDecoder.Mappings.Model;

namespace SetiEncoderDecoder.Mappings.Mapper
{
    public interface IChemicalElementMapper
    {
        public string this[int atomicNumber] { get; }

        public int this[string symbol] { get; }

        public ICollection<string> Symbols { get; }
    }
}
