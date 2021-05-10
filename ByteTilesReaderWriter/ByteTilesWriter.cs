using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    
    /// <summary>
    /// Creates a .bytetiles file from a .mbtiles file
    /// </summary>
    public class ByteTilesWriter
    {
        static long Position;        
        static string OutputFile;
        static ByteRangeMetadata byteRangeMetadata;
        static FileStream FileStream;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">.mbtiles file</param>
        /// <param name="output">.bytetiles file</param>
        public static void ParseMbtiles(string input, string output)
        {
            MBTilesReader mBTilesReader = new(input);
            Position = 0;
            OutputFile = output;
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
            string startByte = byteRange.ToString().PadRight(ByteTiles.StartByteRange, ' ');
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
