using System.Data.SQLite;

namespace ByteTilesReaderWriter
{
    class RowGridData : Row
    {
        public string Key_name { get; set; }
        public string Key_json { get; set; }

        public RowGridData(SQLiteDataReader sQLiteDataReader): base(sQLiteDataReader)
        {
            Key_name = sQLiteDataReader["key_name"].ToString();
            Key_json = sQLiteDataReader["key_json"].ToString();
        }
    }
}
