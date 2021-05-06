using ByteTilesReaderWriter;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace SimpleByteTilesServer.Controler
{
    [Route("tile/{filename}/{z}/{x}/{y}.pbf")]
    [Produces("application/json")]    
    public class TileController : Controller
    {
        private readonly IMemoryCache MemoryCache;

        public TileController(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public ActionResult Get(string filename, int z, int x, int y)
        {
            string filePath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\" + filename + ".bytetiles";
            
            MemoryCache.TryGetValue(filename, out Dictionary<string, string> dictionary);
            ByteTilesReader byteTilesReader = new(filePath);
            byte[] bytes = byteTilesReader.GetTile(z, y, x, dictionary);
            return File(bytes, "application/octet-stream");
        }
    }
}
