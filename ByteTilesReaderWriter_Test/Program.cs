using System;
using System.Collections.Generic;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {

        private static string FilesPath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\";
        private static string ExtractOutputPath = @"C:\Users\Chus\Downloads\";

        static void Main(string[] args)
        {
            //WriteByteTilesFile("europolis");
            //WriteByteTilesFile("satellite-lowres");
            //WriteByteTilesFile("countries-vector");
            //WriteByteTilesFile("countries-raster");
            //WriteByteTilesFile("buildings");

            //ReadByteTilesFile("europolis");
            //ReadByteTilesFile("satellite-lowres");
            //ReadByteTilesFile("countries-vector");
            //ReadByteTilesFile("countries-raster");

            //ExtractByteTilesFiles("countries-vector");
            //ExtractByteTilesFiles("countries-raster");
        }

        static void WriteByteTilesFile(string file)
        {
            string input = FilesPath + file + ".mbtiles";
            string output = FilesPath + file + ".bytetiles";
            ByteTilesWriter.ParseMbtiles(input, output);
        }

        static void ReadByteTilesFile(string file)
        {
            string input = FilesPath + file + ".bytetiles";
            var byteTilesReader = new ByteTilesReader(input);

            var keyValuePairs = byteTilesReader.GetMetadata();
            foreach (KeyValuePair<string, string> entry in keyValuePairs)
            {
                Console.WriteLine(entry.Key + " - " + entry.Value);
            }


            var dictionary = byteTilesReader.GetTilesDictionary();
            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                Console.WriteLine(entry.Key + " - " + entry.Value);
            }

            byte[] byteArray = byteTilesReader.GetTile(16,32060,40846);
            string text = System.Text.Encoding.Default.GetString(byteArray);
            Console.WriteLine(text);
        }

        static void ExtractByteTilesFiles(string file)
        {
            string input = FilesPath + file + ".bytetiles";
            ByteTilesExtractor byteTilesExtractor = new(input);
            byteTilesExtractor.ExtractTiles(ExtractOutputPath);
        }
    }
}
