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

        public ByteRange MetaData { get; set; } // Byte range for metadata json

        public ByteRange TilesDictionary { get; set; } // Byte range for tiles table dictionary

        public ByteRange GridsDictionary { get; set; } // Byte range for grids table dictionary

        public ByteRange GridDataDictionary { get; set; } // Byte range for grid_data dictinary

        public ByteRangeMetadata()
        {

        }

        public ByteRangeMetadata(string json)
        {
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Version = keyValuePairs["version"];
            MetaData = new ByteRange(keyValuePairs["metadata"]);
            TilesDictionary = new ByteRange(keyValuePairs["tiles_dictionary"]);

            if (keyValuePairs.ContainsKey("grids_dictionary"))
            {
                GridsDictionary = new ByteRange(keyValuePairs["grids_dictionary"]);
            }
            if (keyValuePairs.ContainsKey("grid_data_dictionary"))
            {
                GridDataDictionary = new ByteRange(keyValuePairs["grid_data_dictionary"]);
            }
        }

        public string ToJson()
        {
            List<string> list = new();
            list.Add("\"version\": \"" + Version + "\"");
            list.Add("\"metadata\": \"" + MetaData + "\"");
            list.Add("\"tiles_dictionary\": \"" + TilesDictionary + "\"");
            if (GridsDictionary != null)
            {
                list.Add("\"grids_dictionary\": \"" + GridsDictionary + "\"");
            }
            if(GridDataDictionary != null)
            {
                list.Add("\"grid_data_dictionary\": \"" + GridDataDictionary + "\"");
            }

            return "{" + string.Join(",", list.ToArray()) + "}";
        }
    }
}
