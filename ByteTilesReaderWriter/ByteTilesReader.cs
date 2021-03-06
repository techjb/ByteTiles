using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    /// <summary>
    /// Reads metadata and tiles from .bytetiles file.
    /// </summary>
    public class ByteTilesReader
    {
        private readonly string InputFile;        
        private readonly Header Header;

        public ByteTilesReader(string inputFile)
        {
            InputFile = inputFile;
            string json = GetHeader();
            Header = new Header(json);
        }

        public Dictionary<string, string> GetMetadata()
        {
            byte[] byteArray = GetData(Header.MetaData);
            string metadata = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(metadata);
        }

        public Dictionary<string, string> GetTilesDictionary()
        {
            byte[] byteArray = GetData(Header.TilesDictionary);
            string json = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public byte[] GetTile(int x, int y, int z)
        {
            TileKey tileKey = new (x, y, z);
            return GetTile(tileKey);
        }

        public byte[] GetTile(TileKey tileKey)
        {
            Dictionary<string, string> keyValuePairs = GetTilesDictionary();
            return GetTile(tileKey, keyValuePairs);
        }

        public byte[] GetTile(int x, int y, int z, Dictionary<string, string> keyValuePairs)
        {
            TileKey tileKey = new(x, y, z);
            return GetTile(tileKey, keyValuePairs);
        }

        public byte[] GetTile(TileKey tileKey, Dictionary<string, string> keyValuePairs)
        {
            byte[] bytes = Array.Empty<byte>();            
            if (keyValuePairs.ContainsKey(tileKey.ToString()))
            {
                ByteRange byteRange = new(keyValuePairs[tileKey.ToString()]);
                bytes = GetData(byteRange);
            }
            return bytes;
        }

        public byte[] GetData(ByteRange byteRange)
        {
            byte[] byteArray = new byte[byteRange.Length];
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            fileStream.Seek(byteRange.Position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, byteRange.Length);
            return byteArray;
        }

        private string GetHeader()
        {            
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            byte[] byteArray = new byte[ByteTiles.HeaderByteRangeLength];
            fileStream.Seek(-ByteTiles.HeaderByteRangeLength, SeekOrigin.End);
            fileStream.Read(byteArray, 0, ByteTiles.HeaderByteRangeLength);
            string headerByteRange = Encoding.UTF8.GetString(byteArray).Trim();
            ByteRange byteRange = new (headerByteRange);

            byteArray = new byte[byteRange.Length];
            fileStream.Seek(byteRange.Position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, byteRange.Length);
            return Encoding.UTF8.GetString(byteArray);            
        }
    }
}
