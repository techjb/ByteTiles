using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    public class ByteTilesWriter
    {

        public static void ParseMbtiles(string inputFile, string outputFile)
        {
            MBTilesProvider mbTilesProvides = new(inputFile);            
            Parse(mbTilesProvides, outputFile);
        }

        static void Parse(MBTilesProvider mbTilesProvides, string outputFile)
        {
            File.Delete(outputFile);
            long position = 0;
            int counter = 0;
            List<MBTilesRow> list = mbTilesProvides.GetTiles();
            int total = list.Count;
            string percentageDone = string.Empty;
            Dictionary<string, string> dictionaryMap = new();
            object sync = new();
            using (var fileStream = new FileStream(outputFile, FileMode.Append, FileAccess.Write))
            {
                Parallel.ForEach(list,
                    new ParallelOptions() { MaxDegreeOfParallelism = 1 }, 
                    mBTilesRow =>
                    {
                        byte[] byteArray = mBTilesRow.Tile_data;
                        lock (sync)
                        {
                            fileStream.Write(byteArray, 0, byteArray.Length);

                            int length = byteArray.Length;
                            string positionAndLength = position + "/" + length;
                            string tileKey = mBTilesRow.TileKey();
                            dictionaryMap.Add(tileKey, positionAndLength);
                            position += length;

                            counter++;
                            double percentage = counter * 100 / total;
                            string percentageDoneAux = percentage.ToString("#.#");
                            if (!percentageDone.Equals(percentageDoneAux))
                            {
                                percentageDone = percentageDoneAux;
                                Console.WriteLine("Completed " + percentageDone + "%");
                            }
                        }
                    });

                string dictionary = JsonSerializer.Serialize(dictionaryMap);
                byte[] bytes = Encoding.ASCII.GetBytes(dictionary);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            string dictionaryPosition = position.ToString().PadRight(20, ' ');
            File.AppendAllText(outputFile, dictionaryPosition);
        }
    }
}
