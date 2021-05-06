using System;
using System.Collections.Generic;
using System.Text.Json;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {

        private static string path = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\";

        static void Main(string[] args)
        {
            //WriteByteTilesFile("Europolis");
            //WriteByteTilesFile("satellite-lowres-v1.2-z0-z5");
            //WriteByteTilesFile("countries");

            ReadByteTilesFile("Europolis");
            //ReadByteTilesFile("satellite-lowres-v1.2-z0-z5");
            //ReadByteTilesFile("countries");
        }

        static void WriteByteTilesFile(string file)
        {
            string input = path + file + ".mbtiles";
            string output = path + file + ".bytetiles";
            ByteTilesWriter.ParseMbtiles(input, output);
        }

        static void ReadByteTilesFile(string file)
        {
            string input = path + file + ".bytetiles";
            var byteTilesReader = new ByteTilesReader(input);

            string metadata = byteTilesReader.GetMetadata();
            string json = JsonSerializer.Deserialize<dynamic>(metadata);
            Console.WriteLine(json);

            var dictionary = byteTilesReader.GetTilesDictionary();
            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                Console.WriteLine(entry.Key + " - " + entry.Value);
            }

            byte[] byteArray = byteTilesReader.GetTile(16,32060,40846);
            string text = System.Text.Encoding.Default.GetString(byteArray);
            Console.WriteLine(text);
        }
    }
}
