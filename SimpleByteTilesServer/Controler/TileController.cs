using ByteTilesReaderWriter;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.IO.Compression;

namespace SimpleByteTilesServer.Controler
{
    [Route("tile/{filename}/{z}/{x}/{y}.{format}")]    
    public class TileController : Controller
    {
        private readonly IMemoryCache MemoryCache;

        public TileController(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public ActionResult Get(string filename, int z, int x, int y, string format)
        {
            string filePath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\" + filename + ".bytetiles";
            
            MemoryCache.TryGetValue(filename, out Dictionary<string, string> dictionary);
            ByteTilesReader byteTilesReader = new(filePath);            
            byte[] bytes = byteTilesReader.GetTile(z, x, y, dictionary);
            string contentType;
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
                default:
                    {
                        contentType = "image/png";
                    }
                    break;
            }
            return File(bytes, contentType);
        }

        public static byte[] Decompress(byte[] byteArray)
        {
            MemoryStream memoryStream = new(byteArray);
            GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress, true);
            List<byte> bytes = new();

            int bytesRead = gZipStream.ReadByte();
            while (bytesRead != -1)
            {
                bytes.Add((byte)bytesRead);
                bytesRead = gZipStream.ReadByte();
            }
            gZipStream.Flush();
            memoryStream.Flush();
            gZipStream.Close();
            memoryStream.Close();
            return bytes.ToArray();
        }
    }
}
