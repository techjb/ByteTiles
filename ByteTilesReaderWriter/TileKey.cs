namespace ByteTilesReaderWriter
{
    public class TileKey
    {
        public readonly long y;
        public readonly long x;
        public readonly long z;

        public TileKey(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public TileKey(string text)
        {
            string[] values = text.Split('/');
            x = int.Parse(values[0]);
            y = int.Parse(values[1]);
            z = int.Parse(values[2]);
        }

        public override string ToString()
        {
            return x + "/" + y + "/" + z;
        }
    }
}
