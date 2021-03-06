using System;
using System.Data.SQLite;

namespace ByteTilesReaderWriter
{
    internal class Row
    {
        public long Zoom_level { get; set; }
        public long Tile_column { get; set; }
        public long Tile_row { get; set; }

        public Row(SQLiteDataReader sQLiteDataReader)
        {
            Zoom_level = (long)sQLiteDataReader["zoom_level"];
            Tile_column = (long)sQLiteDataReader["tile_column"];
            Tile_row = (long)sQLiteDataReader["tile_row"];            
        }
        public TileKey TileKey()
        {
            return new TileKey(Tile_column, (long)Math.Pow(2, Zoom_level) - Tile_row - 1, Zoom_level);            
        }
    }
}
