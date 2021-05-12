using System;
using System.Collections.Generic;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {

        private static string FilesPath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\";
        private static string ExtractOutputDirectory = @"C:\Users\Chus\Downloads\";

        static void Main(string[] args)
        {
            //WriteByteTilesFile("europolis", false);
            //WriteByteTilesFile("satellite-lowres", false);
            //WriteByteTilesFile("countries-vector", false);
            //WriteByteTilesFile("countries-raster", false);

            //ReadByteTilesFile("europolis");
            ReadByteTilesFile("satellite-lowres");
            //ReadByteTilesFile("countries-vector");
            //ReadByteTilesFile("countries-raster");

            //ExtractByteTilesFiles("countries-vector");
            //ExtractByteTilesFiles("countries-raster");
        }

        static void WriteByteTilesFile(string file, bool decompress)
        {
            string input = FilesPath + file + ".mbtiles";
            string output = FilesPath + file + ".bytetiles";
            ByteTilesWriter.ParseMBTiles(input, output, decompress);
        }

        static void ReadByteTilesFile(string file)
        {
            string input = FilesPath + file + ".bytetiles";
            var byteTilesReader = new ByteTilesReader(input);

            var metadata = byteTilesReader.GetMetadata();
            foreach (KeyValuePair<string, string> entry in metadata)
            {
                Console.WriteLine(entry.Key + " - " + entry.Value);
            }

            var tilesDictionary = byteTilesReader.GetTilesDictionary();
            foreach (KeyValuePair<string, string> tile in tilesDictionary)
            {
                Console.WriteLine(tile.Key + " - " + tile.Value);
            }

            byte[] byteArray = byteTilesReader.GetTile(8015, 6171, 14);
            string text = System.Text.Encoding.Default.GetString(byteArray);
            Console.WriteLine(text);
        }

        static void ExtractByteTilesFiles(string file)
        {
            string input = FilesPath + file + ".bytetiles";
            ByteTilesExtractor byteTilesExtractor = new(input);
            byteTilesExtractor.ExtractTiles(ExtractOutputDirectory);
        }
    }
}
