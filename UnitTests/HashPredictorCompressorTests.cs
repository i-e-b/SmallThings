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
        Console.WriteLine($"{source.Length} to {compressed.Length} ({Percent(source, compressed)}%) to {decompressed.Length}");
        
        var original = Encoding.UTF8.GetString(source); // do this in case unicode normalisation bites.
        var result = Encoding.UTF8.GetString(decompressed);
        Console.WriteLine(result);
        
        Assert.That(result, Is.EqualTo(original));
        
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        var mangled = MakeRepresentativesString(compressed);
        Console.WriteLine(mangled);
    }

    [Test]
    public void compress_and_decompress_work_with_low_entropy_sources()
    {
        var source = Encoding.UTF8.GetBytes(SampleData.BigLowEntropyString);
        
        var compressed = HashPredictCompressor.Compress(source);
        var decompressed = HashPredictCompressor.Decompress(compressed);
        Console.WriteLine($"{source.Length} to {compressed.Length} ({Percent(source, compressed)}%) to {decompressed.Length}");
        
        var original = Encoding.UTF8.GetString(source); // do this in case unicode normalisation bites.
        var result = Encoding.UTF8.GetString(decompressed);
        Console.WriteLine(result);
        
        Assert.That(result, Is.EqualTo(original));
        
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        var mangled = MakeRepresentativesString(compressed);
        Console.WriteLine(mangled);
    }
    
    [Test]
    public void compress_and_decompress_work_with_short_high_entropy_sources()
    {
        var source = Encoding.UTF8.GetBytes(SampleData.ShortHighEntropyString);
        
        var compressed = HashPredictCompressor.Compress(source);
        var decompressed = HashPredictCompressor.Decompress(compressed);
        Console.WriteLine($"{source.Length} to {compressed.Length} ({Percent(source, compressed)}%) to {decompressed.Length}");
        
        var original = Encoding.UTF8.GetString(source); // do this in case unicode normalisation bites.
        var result = Encoding.UTF8.GetString(decompressed);
        Console.WriteLine(result);
        
        Assert.That(result, Is.EqualTo(original));
        
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        var mangled = MakeRepresentativesString(compressed);
        Console.WriteLine(mangled);
    }


    private static string Percent(byte[] source, byte[] compressed)
    {
        var pc = 100.0 * compressed.Length / source.Length;
        return $"{pc:0.0}";
    }
    
    private static string MakeRepresentativesString(byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (var b in bytes)
        {
            if (b == 0x0D || b == 0x0A) sb.Append((char)b);
            else if (b < ' ' || b > '~') sb.Append('_');
            else sb.Append((char)b);
        }
        
        return sb.ToString();
    }
}