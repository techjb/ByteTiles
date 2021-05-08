﻿using System.Collections.Generic;
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
        static FileStream FileStream;


        public static void ParseMbtiles(string inputFile, string outputFile)
        {
            MBTilesReader mBTilesReader = new(inputFile);
            Position = 0;
            OutputFile = outputFile;
            File.Delete(OutputFile);
            byteRangeMetadata = new ByteRangeMetadata();

            FileStream = new FileStream(OutputFile, FileMode.Append, FileAccess.Write);
            WriteTableTiles(mBTilesReader);
            WriteTableMetadata(mBTilesReader);
            ByteRange byteRange = WriteByteRangeMetadata();
            WriteStartByte(byteRange);
            FileStream.Close();
        }
       
        static void WriteTableTiles(MBTilesReader mBTilesReader)
        {
            var tiles = mBTilesReader.GetTiles();        
            Position = 0;
            string percentageDone = string.Empty;                    

            Dictionary<string, string> dictionaryMap = new();
            int total = tiles.Count;                        
            object sync = new();            
            Parallel.ForEach(tiles,
                //new ParallelOptions() { MaxDegreeOfParallelism = 1 }, 
                tilesRow =>
                {                    
                    lock (sync)
                    {
                        ByteRange byteRange = Write(tilesRow.Tile_data);                      
                        string tileKey = tilesRow.TileKey().ToString();
                        dictionaryMap.Add(tileKey, byteRange.ToString());
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
            byteRangeMetadata.TilesDictionary = Write(dictionary);
        }

        static void WriteTableMetadata(MBTilesReader mBTilesReader)
        {
            string metadata = mBTilesReader.GetMetadata();
            byteRangeMetadata.MetaData = Write(metadata);            
        }

        static ByteRange WriteByteRangeMetadata()
        {
            string json = byteRangeMetadata.ToJson();
            return Write(json);            
        }

        static void WriteStartByte(ByteRange byteRange)
        {
            string startByte = byteRange.ToString().PadRight(StartByteLength, ' ');
            byte[] byteArray = Encoding.UTF8.GetBytes(startByte);
            FileStream.Write(byteArray, 0, byteArray.Length);            
        }

        static ByteRange Write(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Write(bytes);
        }
        static ByteRange Write(byte[] bytes)
        {
            FileStream.Write(bytes, 0, bytes.Length);
            ByteRange byteRange = new(Position, bytes.Length);
            Position += bytes.Length;
            return byteRange;
        }
    }
}
