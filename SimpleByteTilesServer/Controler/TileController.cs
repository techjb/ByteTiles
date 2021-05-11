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
        private readonly IMemoryCache MemoryCache;

        public TileController(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public ActionResult Get(string id, int z, int x, int y, string format)
        {
            byte[] bytes = new ByteTilesCache(MemoryCache).GetTile(id, x, y, z);            
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