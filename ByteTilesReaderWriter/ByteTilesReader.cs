using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    public class ByteTilesReader
    {
        private readonly string InputFile;
        private const int StartByteLength = 20;
        private readonly ByteRangeMetadata ByteRangeMetadata;

        public ByteTilesReader(string inputFile)
        {
            InputFile = inputFile;
            string json = GetByteRangeMetadata();
            ByteRangeMetadata = new ByteRangeMetadata(json);
        }

        public string GetMetadata()
        {
            byte[] byteArray = GetData(ByteRangeMetadata.MetaData);
            return Encoding.UTF8.GetString(byteArray);
        }

        public Dictionary<string, string> GetTilesDictionary()
        {
            byte[] byteArray = GetData(ByteRangeMetadata.TilesDictionary);
            string json = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public byte[] GetTile(int z, int y, int x)
        {
            string tileKey = z + "/" + y + "/" + x;
            return GetTile(tileKey);
        }

        public byte[] GetTile(string tileKey)
        {
            Dictionary<string, string> keyValuePairs = GetTilesDictionary();
            return GetTile(tileKey, keyValuePairs);
        }

        public byte[] GetTile(int z, int y, int x, Dictionary<string, string> keyValuePairs)
        {
            string tileKey = z + "/" + y + "/" + x;
            return GetTile(tileKey, keyValuePairs);
        }

        public byte[] GetTile(string tileKey, Dictionary<string, string> keyValuePairs)
        {
            byte[] bytes = Array.Empty<byte>();            
            if (keyValuePairs.ContainsKey(tileKey))
            {
                string byteRange = keyValuePairs[tileKey];
                bytes = GetData(byteRange);
            }
            return bytes;
        }

        private byte[] GetData(string byteRange) 
        {
            string[] values = byteRange.Split("/");
            long item1 = long.Parse(values[0]);
            int item2 = int.Parse(values[1]);
            var tuple = Tuple.Create(item1, item2);
            return GetData(tuple);
        }

        private byte[] GetData(Tuple<long, int> tuple)
        {
            byte[] byteArray = new byte[tuple.Item2];
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            fileStream.Seek(tuple.Item1, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, tuple.Item2);
            return byteArray;
        }

        private string GetByteRangeMetadata()
        {            
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            byte[] byteArray = new byte[StartByteLength];
            fileStream.Seek(-StartByteLength, SeekOrigin.End);
            fileStream.Read(byteArray, 0, StartByteLength);
            string byteTilesMetadataPosition = Encoding.UTF8.GetString(byteArray).Trim();
            long position = long.Parse(byteTilesMetadataPosition);
            int length = (int)(fileStream.Length - position - StartByteLength);

            byteArray = new byte[length];
            fileStream.Seek(position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, length);
            return Encoding.UTF8.GetString(byteArray);            
        }
    }
}
