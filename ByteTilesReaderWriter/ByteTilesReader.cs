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
        public ByteTilesReader(string inputFile)
        {
            InputFile = inputFile;
        }

        public ByteTilesMetadata GetByteTilesMetadata()
        {
            string json = GetByteTilesMetadataJson();
            return new ByteTilesMetadata(json);
        }

        public string GetMetadata()
        {
            ByteTilesMetadata byteTilesMetadata = GetByteTilesMetadata();

            byte[] byteArray = new byte[byteTilesMetadata.MetaData.Item2];
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            fileStream.Seek(byteTilesMetadata.MetaData.Item1, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, byteTilesMetadata.MetaData.Item2);
            return Encoding.UTF8.GetString(byteArray);
        }

        private string GetByteTilesMetadataJson()
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
            //return JsonSerializer.Deserialize<Dictionary<string, string>>(map);
        }
    }
}
