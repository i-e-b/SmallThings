namespace Algorithms;

/// <summary>
/// General purpose compressor by PPM (Prediction by Partial Matching)
/// A predictor/corrector using a hash table.
/// This implementation is very basic, but is also simple to follow.
/// This does NOT guarantee a smaller-or-equal output size.
/// <p></p>
/// Based on
/// https://bugfix-66.com/5e54291e3c874c3d25ba72a8766e63d4b33e0c9674d607d4fe24d1367b90b298
/// https://bugfix-66.com/d548f3abf6faa823a829e4c770a8babca648a5fdcbf69f9a7ba385bb3199f8f0
/// <p></p>
/// See
/// https://en.wikipedia.org/wiki/Prediction_by_partial_matching
/// </summary>
public class HashPredictCompressor
{
    private const int order = 3;
    private const ulong mixer = 0xff51afd7ed558ccdUL;
    private const int numBits = 12; // how many entries in hash table (2^numBits)
    
    public static byte[] Compress(byte[] from)
    {
        var ctrl = 0; // bit flags for predicted or not
        var bit = 128; // bit mask for ctrl
        var loc = 0; // where we are updating the control byte
        var to = new List<byte>(); // output bytes (needs at least 8 bytes of random access)
        var ctx = 0UL; // Hash context. This is data mixed together for prediction
        var lut = new byte[1 << numBits]; // Hash table, will be built from input data

        to.Add(0);                    // place a byte to hold the ctrl flags
        foreach (var next in from)    // For each byte, processed in order
        {
            var hash = ctx * mixer;   // Mix the hash key with context
            hash >>= 64 - numBits;
            var pred = lut[hash];     // Look up the predicted value

            if (pred == next)         // If we predicted the value correctly
            {
                ctrl += bit;          // Then set the control bit, but don't write the data value
            }
            else                      // If the prediction was wrong
            {
                lut[hash] = next;     // Then update our hash table for next prediction
                to.Add(next);         // And append the data value, without setting the control bit
            }
            bit >>= 1;                // move to next control bit
            if (bit == 0)             // If we've run out of space in the control byte
            {
                to[loc] = (byte)ctrl; // write the control byte into the output
                ctrl = 0;             // clear the control bits
                bit = 128;            // reset the current bit
                to.Add(0);            // write a new control byte (we will update later)
                loc = to.Count - 1;   // record the location of the control byte
            }
            ctx <<= 8;                // make space in context for data
            ctx += next;              // add data to context
            ctx &= (1 << order * 8) - 1; // mix
        }
        
        to[loc] = (byte)ctrl;         // write final control byte
        return to.ToArray();          // return final output
    }

    public static byte[] Decompress(byte[] compressed)
    {
        throw new NotImplementedException();
    }
}