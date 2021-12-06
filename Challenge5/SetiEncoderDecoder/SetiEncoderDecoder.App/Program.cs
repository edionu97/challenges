using System;
using System.Threading.Tasks;
using SetiEncoderDecoder.Mappings.Mapper.Impl;
using SetiEncoderDecoder.Decoder.Decoding.Impl;
using SetiEncoderDecoder.Encoder.Encoding.Impl;

namespace SetiEncoderDecoder.App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //create mapper
            var mapper =
                await WebBasedChemicalElementMapper.CreateAsync();

            //create the objects
            var encoder = new SetiEncoder(mapper);
            var decoder = new SetiDecoder(mapper);

            Console.Write("Enter input: ");
            var inputSeq = Console.ReadLine();

            var cmd = inputSeq?.Contains("+") == true
                ? "decode"
                : "encode";

            //get the answer
            var answer = cmd switch
            {
                "encode" or "e" => encoder.EncodeSequence(inputSeq),
                "decode" or "d" => decoder.DecodeSequence(inputSeq),
                _ => "Wrong input"
            };

            Console.WriteLine($"Result: {answer}");
        }
    }
}
