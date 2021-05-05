using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace ByteTilesReaderWriter
{
    internal class MBTilesProvider
    {
        public string MinZoom { get; private set; }
        public string MaxZoom { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Description { get; private set; }
        public string MBTilesVersion { get; private set; }
        public string Path { get; private set; }
        public string Bounds { get; private set; }
        public string Center { get; private set; }
        public string Type { get; private set; }
        public string Format{ get; private set; }
        public string Json{ get; private set; }
        public string Generator { get; private set; }
        public string GeneratorOptions { get; private set; }

        public MBTilesProvider(string path)
        {
            Path = path;
            LoadMetadata();
        }

        public void LoadMetadata()
        {
            try
            {
                using SQLiteConnection conn = new(string.Format("Data Source={0};Version=3;", Path));
                conn.Open();
                using SQLiteCommand cmd = new() { Connection = conn, CommandText = "SELECT * FROM metadata;" };
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string value = "";
                    switch (name.ToLower())
                    {
                        case "bounds":
                            Bounds = reader["value"].ToString();
                            break;
                        case "center":
                            Center = reader["value"].ToString();
                            break;
                        case "minzoom":
                            MinZoom = reader["value"].ToString();
                            break;
                        case "maxzoom":
                            MaxZoom = reader["value"].ToString();
                            break;
                        case "name":
                            Name = reader["value"].ToString();
                            break;
                        case "description":
                            Description = reader["value"].ToString();
                            break;
                        case "version":
                            Version = reader["value"].ToString();
                            break;
                        case "type":
                            Type = reader["value"].ToString();
                            break;
                        case "format":
                            Format = reader["value"].ToString();
                            break;
                        case "json":
                            Json = reader["value"].ToString();
                            break;
                        case "generator":
                            Generator = reader["value"].ToString();
                            break;
                        case "generator_options":
                            GeneratorOptions = reader["value"].ToString();
                            break;
                        default:
                            {
                                value = reader["value"].ToString();
                            }
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        public List<MBTilesRow> GetTiles()
        {
            Console.WriteLine("Opening MBTiles file..");
            List<MBTilesRow> list = new();            
            try
            {
                using SQLiteConnection conn = new(string.Format("Data Source={0};Version=3;", Path));
                conn.Open();
                string query = "SELECT * FROM tiles;";
                using SQLiteCommand cmd = new()
                {
                    Connection = conn,
                    CommandText = query
                };
                SQLiteDataReader reader = cmd.ExecuteReader();
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
