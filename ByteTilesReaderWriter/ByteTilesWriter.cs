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
        static long Position;
        const int StartByteLength = 40;
        static string OutputFile;
        static ByteRangeMetadata byteRangeMetadata;
        
        public static void ParseMbtiles(string inputFile, string outputFile)
        {
            MBTilesReader mBTilesReader = new(inputFile);
            Position = 0;
            OutputFile = outputFile;
            File.Delete(OutputFile);
            byteRangeMetadata = new ByteRangeMetadata();

            using var fileStream = new FileStream(OutputFile, FileMode.Append, FileAccess.Write);
            WriteTableTiles(mBTilesReader, fileStream);
            WriteTableMetadata(mBTilesReader, fileStream);
            ByteRange byteRange = WriteByteRangeMetadata(fileStream);
            WriteStartByte(fileStream, byteRange);
        }
       
        static void WriteTableTiles(MBTilesReader mBTilesReader, FileStream fileStream)
        {
            var tiles = mBTilesReader.GetTiles();
            if (tiles.Count.Equals(0))
            {
                return;
            }
            //int counter = 0;
            Position = 0;
            string percentageDone = string.Empty;                    

            Dictionary<string, string> dictionaryMap = new();
            int total = tiles.Count;                        
            object sync = new();            
            Parallel.ForEach(tiles,
                //new ParallelOptions() { MaxDegreeOfParallelism = 1 }, 
                tilesRow =>
                {
                    byte[] byteArray = tilesRow.Tile_data;
                    lock (sync)
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);

                        int length = byteArray.Length;
                        string positionAndLength = Position + "/" + length;
                        string tileKey = tilesRow.TileKey();
                        dictionaryMap.Add(tileKey, positionAndLength);
                        Position += length;

                        //counter++;
                        //double percentage = counter * 100 / total;
                        //string percentageDoneAux = percentage.ToString("#.#");
                        //if (!percentageDone.Equals(percentageDoneAux))
                        //{
                        //    percentageDone = percentageDoneAux;
                        //    Console.WriteLine("Completed " + percentageDone + "%");
                        //}
                    }
                });

            string dictionary = JsonSerializer.Serialize(dictionaryMap);
            byte[] bytes = Encoding.UTF8.GetBytes(dictionary);
            fileStream.Write(bytes, 0, bytes.Length);
            byteRangeMetadata.TilesDictionary = new ByteRange(Position, bytes.Length);            
            Position += bytes.Length;
        }

        static void WriteTableMetadata(MBTilesReader mBTilesReader, FileStream fileStream)
        {
            string medatata = mBTilesReader.GetMetadata();
            if (medatata.Equals(string.Empty))
            {
                return;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(medatata);
            fileStream.Write(bytes, 0, bytes.Length);
            byteRangeMetadata.MetaData = new ByteRange(Position, bytes.Length);
            Position += bytes.Length;
        }

        static ByteRange WriteByteRangeMetadata(FileStream fileStream)
        {
            string json = byteRangeMetadata.ToJson();
            byte[] byteArray = Encoding.UTF8.GetBytes(json);            
            fileStream.Write(byteArray, 0, byteArray.Length);
            ByteRange byteRange = new(Position, byteArray.Length);
            Position += byteArray.Length;
            return byteRange;
        }

        static void WriteStartByte(FileStream fileStream, ByteRange byteRange)
        {
            string startByte = byteRange.ToString().PadRight(StartByteLength, ' ');
            byte[] byteArray = Encoding.UTF8.GetBytes(startByte);
            fileStream.Write(byteArray, 0, byteArray.Length);            
        }
    }
}
