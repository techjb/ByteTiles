using System.Data.SQLite;

namespace ByteTilesReaderWriter
{
    class RowTiles: Row
    {
        public byte[] Tile_data { get; set; }

        public RowTiles(SQLiteDataReader sQLiteDataReader): base(sQLiteDataReader)
        {
            Tile_data = sQLiteDataReader["tile_data"] as byte[];
        }
    }
}
