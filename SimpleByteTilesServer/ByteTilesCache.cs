using ByteTilesReaderWriter;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleByteTilesServer
{
    public class ByteTilesCache
    {
        IMemoryCache MemoryCache;

        public ByteTilesCache(IMemoryCache memoryCache) 
        {
            MemoryCache = memoryCache;
        }
        public void SetTilesDictionary(string file)
        {            
            ByteTilesReader byteTilesReader = new(file);
            Dictionary<string, string> dictionary = byteTilesReader.GetTilesDictionary();
            string id = Path.GetFileNameWithoutExtension(file);
            MemoryCache.Set(id, dictionary);
        }

        public Dictionary<string, string> GetTilesDictionary(string id)
        {
            MemoryCache.TryGetValue(id, out Dictionary<string, string> dictionary);
            return dictionary;
        }
    }
}
