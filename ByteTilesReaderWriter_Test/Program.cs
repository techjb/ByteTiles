using System;
using ByteTilesReaderWriter;

namespace ByteTilesReaderWriter_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            string output = "";
            ByteTilesWriter.ParseMbtiles(input, output);
        }
    }
}
