using System.Text;
using Algorithms;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class HashPredictorCompressorTests
{

    [Test]
    public void compressor_can_take_a_byte_stream_and_generate_a_byte_stream()
    {
        var source = Encoding.UTF8.GetBytes(SampleData.BigString);
        var result = HashPredictCompressor.Compress(source);
        
        Console.WriteLine($"{source.Length} to {result.Length}");
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void decompressor_can_take_a_compressed_byte_stream_and_generate_the_original_byte_stream()
    {
        var source = Encoding.UTF8.GetBytes(SampleData.BigString);
        
        var compressed = HashPredictCompressor.Compress(source);
        var decompressed = HashPredictCompressor.Decompress(compressed);
        Console.WriteLine($"{source.Length} to {compressed.Length} to {decompressed.Length}");
        
        var result = Encoding.UTF8.GetString(decompressed);
        
        Assert.That(result, Is.EqualTo(source));
    }
}