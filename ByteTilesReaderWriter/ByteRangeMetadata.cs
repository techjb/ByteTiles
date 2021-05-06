using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    /// <summary>
    /// List of byte range (start byte position and length) where starting to read each table.
    /// </summary>
    public class ByteRangeMetadata
    {
        public string Version = "1.0";

        public Tuple<long, int> MetaData { get; set; } // Byte range for metadata json

        public Tuple<long,int> TilesDictionary { get; set; } // Byte range for tiles table dictionary

        public Tuple<long, int> GridsDictionary { get; set; } // Byte range for grids table dictionary

        public Tuple<long, int> GridDataDictionary { get; set; } // Byte range for grid_data dictinary

        public ByteRangeMetadata()
        {

        }

        public ByteRangeMetadata(string json)
        {
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (keyValuePairs.ContainsKey("version"))
            {
                Version = keyValuePairs["version"];
            }
            if (keyValuePairs.ContainsKey("metadata"))
            {
                MetaData = ByteRange(keyValuePairs["metadata"]);
            }
            if (keyValuePairs.ContainsKey("tiles_dictionary"))
            {
                TilesDictionary = ByteRange(keyValuePairs["tiles_dictionary"]);
            }
            if (keyValuePairs.ContainsKey("grids_dictionary"))
            {
                GridsDictionary = ByteRange(keyValuePairs["grids_dictionary"]);
            }
            if (keyValuePairs.ContainsKey("grid_data_dictionary"))
            {
                GridDataDictionary = ByteRange(keyValuePairs["grid_data_dictionary"]);
            }
        }

        public string ToJson()
        {
            List<string> list = new();
            list.Add("\"version\": \"" + Version + "\"");
            list.Add("\"metadata\": \"" + ByteRange(MetaData) + "\"");
            list.Add("\"tiles_dictionary\": \"" + ByteRange(TilesDictionary) + "\"");
            if (GridsDictionary != null)
            {
                list.Add("\"grids_dictionary\": \"" + ByteRange(GridsDictionary) + "\"");
            }
            if(GridDataDictionary != null)
            {
                list.Add("\"grid_data_dictionary\": \"" + ByteRange(GridDataDictionary) + "\"");
            }

            return "{" + string.Join(",", list.ToArray()) + "}";
        }

        public void SetMetadata(long position, int length)
        {
            MetaData = ByteRange(position, length);
        }

        public void SetTilesDictionary(long position, int length)
        {
            TilesDictionary = ByteRange(position, length);
        }

        public void SetGridsDictionary(long position, int length)
        {
            GridsDictionary = ByteRange(position, length);
        }

        public void SetGridDataDictionary(long position, int length)
        {
            GridDataDictionary = ByteRange(position, length);
        }


        private static string ByteRange(Tuple<long, int> tuple)
        {
            return tuple.Item1 + "/" + tuple.Item2;
        }

        private static Tuple<long, int> ByteRange(string text)
        {
            string[] values = text.Split("/");
            long item1 = long.Parse(values[0]);
            int item2 = int.Parse(values[1]);
            return Tuple.Create(item1, item2);                 
        }

        private static Tuple<long, int> ByteRange(long position, int length)
        {
            return Tuple.Create(position, length);
        }
    }
}
