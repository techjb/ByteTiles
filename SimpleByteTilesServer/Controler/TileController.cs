using ByteTilesReaderWriter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SimpleByteTilesServer.Controler
{
    [Route("tile/{id}/{z}/{x}/{y}.{format}")]
    public class TileController : Controller
    {
        private readonly string filePath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\";
        private readonly IMemoryCache MemoryCache;

        public TileController(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public ActionResult Get(string id, int z, int x, int y, string format)
        {
            string file = filePath + id + ".bytetiles";
            
            Dictionary<string, string> tiles = new ByteTilesCache(MemoryCache).GetTilesDictionary(id);
            ByteTilesReader byteTilesReader = new(file);
            byte[] bytes = byteTilesReader.GetTile(x, y, z, tiles);
            string contentType = string.Empty;
            switch (format)
            {
                case "pbf":
                    {
                        bytes = Decompress(bytes);
                        contentType = "application/x-protobuf";
                    }
                    break;

                case "png":
                    {
                        contentType = "image/png";
                    }
                    break;

                case "jpg":
                    {
                        contentType = "image/jpeg";
                    }
                    break;               
            }
            return File(bytes, contentType);
        }

        public static byte[] Decompress(byte[] @byte)
        {
            MemoryStream memoryStream = new(@byte);
            GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress, true);
            List<byte> bytes = new();

            int bytesRead = gZipStream.ReadByte();
            while (bytesRead != -1)
            {
                bytes.Add((byte)bytesRead);
                bytesRead = gZipStream.ReadByte();
            }
            return bytes.ToArray();
        }
    }
}