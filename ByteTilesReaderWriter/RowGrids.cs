using System.Data.SQLite;

namespace ByteTilesReaderWriter
{
    class RowGrids : Row
    {
        public byte[] Grid { get; set; }

        public RowGrids(SQLiteDataReader sQLiteDataReader): base(sQLiteDataReader)
        {
            Grid = sQLiteDataReader["grid"] as byte[];
        }
    }
}
