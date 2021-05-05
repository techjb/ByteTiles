using System;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\Europolis.mbtiles";
            string output = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\Europolis.bytetiles"; ;
            ByteTilesWriter.ParseMbtiles(input, output);

            //var byteTilesReader = new ByteTilesReader(output);
            //var dictionary = byteTilesReader.GetDictionary();
        }
    }
}
