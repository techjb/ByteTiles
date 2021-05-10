using ByteTilesReaderWriter;
using Microsoft.Extensions.Caching.Memory;
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
            string id = Path.GetFileNameWithoutExtension(file);
            MemoryCache.Set(id, tilesDictionary);
        }

        public Dictionary<string, string> GetTilesDictionary(string id)
        {
            MemoryCache.TryGetValue(id, out Dictionary<string, string> dictionary);
            return dictionary;
        }
    }
}
