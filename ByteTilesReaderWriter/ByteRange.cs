namespace ByteTilesReaderWriter
{
    public class ByteRange
    {
        public readonly long Position;
        public readonly int Length;
        const char Separator = '-';

        public ByteRange(long position, int length)
        {
            Position = position;
            Length = length;
        }
        public ByteRange(string text)
        {
            string[] values = text.Split(Separator);
            Position = long.Parse(values[0]);
            Length = int.Parse(values[1]);
        }

        public override string ToString()
        {
            return Position + Separator.ToString() + Length;
        }
    }
}
