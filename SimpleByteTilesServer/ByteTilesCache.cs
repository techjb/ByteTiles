using ByteTilesReaderWriter;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleByteTilesServer
{
    public class ByteTilesCache
    {
        readonly IMemoryCache MemoryCache;

        public ByteTilesCache(IMemoryCache memoryCache) 
        {
            MemoryCache = memoryCache;
        }
        public void SetTilesDictionary(string file)
        {            
            ByteTilesReader byteTilesReader = new(file);
            Dictionary<string, string> tilesDictionary = byteTilesReader.GetTilesDictionary();
            Tuple<string, Dictionary<string, string>> tuple = new(file, tilesDictionary);
            string id = Path.GetFileNameWithoutExtension(file);
            MemoryCache.Set(id, tuple);
        }

        public byte[] GetTile(string id, int x, int y, int z)
        {
            MemoryCache.TryGetValue(id, out Tuple<string, Dictionary<string, string>> tuple);
            string file = tuple.Item1;
            Dictionary<string, string> dictionary = tuple.Item2;

            ByteTilesReader byteTilesReader = new(file);
            return byteTilesReader.GetTile(x, y, z, dictionary);
        }
    }
}
