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
        private const int DictionaryPositionSize = 20;
        public ByteTilesReader(string inputFile)
        {
            InputFile = inputFile;
        }

        public Dictionary<string, string> GetDictionary()
        {
            
            using FileStream fileStream = new(InputFile, FileMode.Open, FileAccess.Read);
            byte[] byteArray = new byte[DictionaryPositionSize];
            fileStream.Seek(-DictionaryPositionSize, SeekOrigin.End);
            fileStream.Read(byteArray, 0, DictionaryPositionSize);
            string dictionaryPosition = Encoding.UTF8.GetString(byteArray).Trim();
            long position = long.Parse(dictionaryPosition);
            int length = (int)(fileStream.Length - position - DictionaryPositionSize);

            byteArray = new byte[length];
            fileStream.Seek(position, SeekOrigin.Begin);
            fileStream.Read(byteArray, 0, length);
            string map = Encoding.UTF8.GetString(byteArray);            
            return JsonSerializer.Deserialize<Dictionary<string, string>>(map);
        }
    }
}
