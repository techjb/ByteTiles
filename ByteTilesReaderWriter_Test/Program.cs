using System;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {

        private static string path = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\";

        static void Main(string[] args)
        {
            //WriteByteTilesFile1("Europolis");
            //WriteByteTilesFile2("satellite-lowres-v1.2-z0-z5");

            ReadByteTilesFile1("Europolis");
        }

        static void WriteByteTilesFile1(string file)
        {
            string input = path + file + ".mbtiles";
            string output = path + file + ".bytetiles";
            ByteTilesWriter.ParseMbtiles(input, output);
        }

        static void WriteByteTilesFile2(string file)
        {
            string input = path + file + ".mbtiles";
            string output = path + file + ".bytetiles";
            ByteTilesWriter.ParseMbtiles(input, output);            
        }

        static void ReadByteTilesFile1(string file)
        {
            string input = path + file + ".bytetiles";
            var byteTilesReader = new ByteTilesReader(input);
            var byteTilesMetadata = byteTilesReader.GetByteTilesMetadata();
            string metadata = byteTilesReader.GetMetadata();
        }
    }
}
