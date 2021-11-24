using System.Threading.Tasks;
using SetiEncoderDecoder.Decoder.Decoding.Impl;
using SetiEncoderDecoder.Encoder.Encoding.Impl;
using SetiEncoderDecoder.Mappings.Mapper;
using SetiEncoderDecoder.Mappings.Mapper.Impl;
using Xunit;

namespace SetiEncoderDecoder.UnitTests
{
    public class UnitTestsFixture
    {
        public IChemicalElementMapper ElementMapper { get; private set; }

        public async Task Create()
        {
            if (ElementMapper != null)
            {
                return;
            }

            ElementMapper = await WebBasedChemicalElementMapper.CreateAsync();
        }
    }


    public class EncoderDecoderTests : IClassFixture<UnitTestsFixture>
    {
        private readonly UnitTestsFixture _fixture;

        public EncoderDecoderTests(UnitTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("+++++++++++++++++00000000000000000-----------------" +
                    "+++++++++++++++++00000000000000000-----------------" +
                    "++++++++00000000--------+-++++++000000------", "CHCl2O")]
        [InlineData("+-+-", "H2")]
        [InlineData("++++++++00000000--------++++++++00000000--------", "O2")]
        [InlineData("+-+-++++++++00000000--------", "H2O")]
        [InlineData("++++++++00000000--------+-+-", "H2O")]
        [InlineData("++++++000000------++++++000000------++++++000000------++++++000000------" +
                    "++++++000000------++++++000000------+-+-+-+-+-+-+-+-+-+-+-+-++++++++00000000--------" +
                    "++++++++00000000--------++++++++00000000--------++++++++00000000--------" +
                    "++++++++00000000--------++++++++00000000--------", "C6H12O6")]
        [InlineData("+++++++++++++++++00000000000000000-----------------+++++++++++00000000000-----------", "ClNa")]
        public async Task CheckDecoding(string input, string output)
        {
            //ensure that the element mapper is created
            await _fixture.Create();

            //create the decoder
            var sut = new SetiDecoder(_fixture.ElementMapper);

            //decode value
            var decodedValue = sut.DecodeSequence(input);

            //assert equal
            Assert.Equal(output, decodedValue);
        }

        [Theory]
        [InlineData("H2O", "+-+-++++++++00000000--------")]
        [InlineData("He", "++00--")]
        [InlineData("KOH", "+++++++++++++++++++0000000000000000000-------------------++++++++00000000--------+-")]
        [InlineData("NaOH", "+++++++++++00000000000-----------++++++++00000000--------+-")]
        [InlineData("HeH2", "++00--+-+-")]
        [InlineData("ClCO2", "+++++++++++++++++00000000000000000-----------------++++++000000------++++++++00000000--------++++++++00000000--------")]
        public async Task CheckEncoding(string input, string output)
        {
            //ensure that the element mapper is created
            await _fixture.Create();

            //create the decoder
            var sut = new SetiEncoder(_fixture.ElementMapper);

            //decode value
            var encodedValue = sut.EncodeSequence(input);

            //assert equal
            Assert.Equal(output, encodedValue);
        }
        
    }
}
