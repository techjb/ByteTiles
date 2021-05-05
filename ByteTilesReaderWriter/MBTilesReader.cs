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

        public List<RowTiles> GetTiles()
        {
            List<RowTiles> list = new();            
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
                    var mbTileRow = new RowTiles(reader);
                    list.Add(mbTileRow);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return list;
        }

        public List<RowGrids> GetGrids()
        {
            List<RowGrids> list = new();
            try
            {
                using SQLiteConnection connection = new(string.Format("Data Source={0};Version=3;", File));
                connection.Open();
                string query = "SELECT * FROM grids;";
                using SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = query
                };
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var row = new RowGrids(reader);
                    list.Add(row);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return list;
        }

        public List<RowGridData> GetGridData()
        {
            List<RowGridData> list = new();
            try
            {
                using SQLiteConnection connection = new(string.Format("Data Source={0};Version=3;", File));
                connection.Open();
                string query = "SELECT * FROM grid_data;";
                using SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = query
                };
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var row = new RowGridData(reader);
                    list.Add(row);
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
