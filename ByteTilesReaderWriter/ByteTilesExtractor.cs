using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ByteTilesReaderWriter
{
    /// <summary>
    /// Extract files contained in .mbtiles file in a output folder
    /// </summary>
    public class ByteTilesExtractor
    {
        private readonly ByteTilesReader ByteTilesReader;
        private readonly string InputFile;
        private static string OutputPath;

        public ByteTilesExtractor(string inputFile)
        {
            InputFile = inputFile;
            ByteTilesReader = new(InputFile);
        }

        public void ExtractTiles(string outputPath)
        {
            OutputPath = outputPath + Path.GetFileNameWithoutExtension(InputFile) + "\\";
            DeleteDirectory();

            var keyValuePairs = ByteTilesReader.GetMetadata();
            string format = keyValuePairs["format"];

            var tilesDictionary = ByteTilesReader.GetTilesDictionary();
            Parallel.ForEach(tilesDictionary, keyValuePair =>
            {
                ExtractTile(keyValuePair, format);
            });
        }

        private void ExtractTile(KeyValuePair<string,string> keyValuePair, string format)
        {
            TileKey tileKey = new(keyValuePair.Key);
            ByteRange byteRange = new(keyValuePair.Value);
            byte[] bytes = ByteTilesReader.GetData(byteRange);
            if (format.Equals("pbf"))
            {
                bytes = Decompress(bytes);
            }
            string directory = OutputPath + tileKey.z + "\\" + tileKey.x + "\\";
            Directory.CreateDirectory(directory);
            string file = directory + tileKey.y + "." + format;
            File.WriteAllBytes(file, bytes);
        }

        private static void DeleteDirectory()
        {
            if (!Directory.Exists(OutputPath))
            {
                return;
            }
            DirectoryInfo directoryInfo = new(OutputPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }
            foreach (DirectoryInfo subdirectory in directoryInfo.GetDirectories())
            {
                subdirectory.Delete(true);
            }
            directoryInfo.Delete(true);
        }

        public static byte[] Decompress(byte[] byteArray)
        {
            MemoryStream memoryStream = new(byteArray);
            GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress, true);
            List<byte> bytes = new();

            int bytesRead = gZipStream.ReadByte();
            while (bytesRead != -1)
            {
                bytes.Add((byte)bytesRead);
                bytesRead = gZipStream.ReadByte();
            }           
            return bytes.ToArray();
        }
    }
}
