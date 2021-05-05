using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    public class ByteTilesMetadata
    {
        public string Version = "1.0";

        public Tuple<long, int> MetaData { get; set; }

        public Tuple<long,int> TilesDictionary { get; set; }

        public Tuple<long, int> GridsDictionary { get; set; }

        public Tuple<long, int> GridDataDictionary { get; set; }

        public ByteTilesMetadata()
        {

        }

        public ByteTilesMetadata(string json)
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
    }
}
