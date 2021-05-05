using System;

namespace ByteTilesReaderWriter
{
    internal class MBTilesRow
    {
        public long Zoom_level { get; set; }
        public long Tile_column { get; set; }
        public long Tile_row { get; set; }
        public byte[] Tile_data { get; set; }

        public string TileKey()
        {
            long tile_row_aux = (long)Math.Pow(2, Zoom_level) - Tile_row - 1; // from tms to google maps
            return Zoom_level + "/" + Tile_column + "/" + tile_row_aux;
        }
    }
}
