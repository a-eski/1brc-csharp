using _1brc_csharp_implementations.Models;

namespace _1brc_csharp_implementations.Common;

public ref struct HashTable
{
    public string?[] Keys;
    public WeatherValues[] Values;

    // private static ulong Hash(string key)
    // {
    //     const ulong fnvOffset = 2166136261;
    //     ulong i;
    //     for (i = fnvOffset; i < (ulong)key.Length; i++)
    //     {
    //         i += (i << 1) + (i << 4) + (i << 7) + (i << 8) + (i << 24);
    //         i ^= key[(int)i]; 
    //     }
    //
    //     return i;
    // }
    
    private static int Hash(string key)
    {
        return key[0].GetHashCode() + key[1].GetHashCode() ^ key[1].GetHashCode();
    }

    public ref WeatherValues AddOrGet(string key)
    {
        var hash = Hash(key);
        const int mask = 127;
        var step = (hash >> 13) | 1;
        for (var i = hash;;)
        {
            i = (i + step) & mask;
            if (Keys[i] == null)
            {
                Keys[i] = key;
                return ref Values[i];
            }
            if (Keys[i] == key)
            {
                return ref Values[i];
            }
        }
    }
}