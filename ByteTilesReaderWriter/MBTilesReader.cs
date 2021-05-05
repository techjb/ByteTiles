using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.Json;

namespace ByteTilesReaderWriter
{
    internal class MBTilesReader
    {
        readonly string File;
        public MBTilesReader(string file)
        {
            File = file;            
        }

        public string GetMetadata()
        {
            Dictionary<string, string> dictionary = new();
            try
            {
                using SQLiteConnection connection = new(string.Format("Data Source={0};Version=3;", File));
                connection.Open();
                using SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = "SELECT * FROM metadata;"
                };
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string value = reader["value"].ToString();
                    dictionary.Add(name, value);
                }
            }
            catch
            {
            }
            return JsonSerializer.Serialize(dictionary);            
        }

        public List<MBTilesRow> GetTiles()
        {
            List<MBTilesRow> list = new();            
            try
            {
                using SQLiteConnection connection = new(string.Format("Data Source={0};Version=3;", File));
                connection.Open();
                string query = "SELECT * FROM tiles;";
                using SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = query
                };
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var mbTileRow = new MBTilesRow()
                    {
                        Zoom_level = (long)reader["zoom_level"],
                        Tile_column = (long)reader["tile_column"],
                        Tile_row = (long)reader["tile_row"],
                        Tile_data = reader["tile_data"] as byte[],
                    };
                    list.Add(mbTileRow);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return list;
        }
    }
}
