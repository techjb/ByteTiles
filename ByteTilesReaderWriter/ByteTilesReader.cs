using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ByteTilesReaderWriter
{
    public class ByteTilesReader
    {
        private readonly string InputFile;
        private const int StartByteLength = 40;
        private readonly ByteRangeMetadata ByteRangeMetadata;

        public ByteTilesReader(string inputFile)
        {
            InputFile = inputFile;
            string json = GetByteRangeMetadata();
            ByteRangeMetadata = new ByteRangeMetadata(json);
        }

        public Dictionary<string, string> GetMetadata()
        {
            byte[] byteArray = GetData(ByteRangeMetadata.MetaData);
            string metadata = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(metadata);
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
                ByteRange byteRange = new(keyValuePairs[tileKey]);
                bytes = GetData(byteRange);
            }
            return bytes;
        }

        private byte[] GetData(ByteRange byteRange)
        {
            byte[] byteArray = new byte[byteRange.Length];
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            fileStream.Seek(byteRange.Position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, byteRange.Length);
            return byteArray;
        }

        private string GetByteRangeMetadata()
        {            
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            byte[] byteArray = new byte[StartByteLength];
            fileStream.Seek(-StartByteLength, SeekOrigin.End);
            fileStream.Read(byteArray, 0, StartByteLength);
            string byteRangeMetadata = Encoding.UTF8.GetString(byteArray).Trim();
            ByteRange byteRange = new (byteRangeMetadata);

            byteArray = new byte[byteRange.Length];
            fileStream.Seek(byteRange.Position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, byteRange.Length);
            return Encoding.UTF8.GetString(byteArray);            
        }

        public void ExtractTiles(string path)
        {
            path += Path.GetFileNameWithoutExtension(InputFile) + "\\";
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }            

            var keyValuePairs = GetMetadata();
            string format = keyValuePairs["format"];

            var tilesDictionary = GetTilesDictionary();
            Parallel.ForEach(tilesDictionary, keyValuePair => 
            {
                ByteRange byteRange = new(keyValuePair.Value);
                byte[] bytes = GetData(byteRange);
                if (format.Equals("pbf"))
                {
                    bytes = Decompress(bytes);
                }
                string[] keys = keyValuePair.Key.Split('/');
                string directory = path + keys[0] + "\\" + keys[1] + "\\";
                Directory.CreateDirectory(directory);
                string file = directory + keys[2] + "." + format;
                File.WriteAllBytes(file, bytes);
            });            
        }

        private static void DeleteDirectory(string path)
        {
            DirectoryInfo directoryInfo = new(path);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            directoryInfo.Delete(true);
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
