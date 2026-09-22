namespace Cellive.Cil.Metadata
{
    public sealed class BlobHeap
    {
        private readonly List<byte[]> blobs = new() { Array.Empty<byte>() };

        public IReadOnlyList<byte[]> Blobs => blobs;
        public int Count => blobs.Count;

        public void Read(BinaryReader reader, uint offset, uint size)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            var end = offset + size;
            while (reader.BaseStream.Position < end)
            {
                var length = ReadCompressedUInt32(reader);
                if (length == 0)
                {
                    blobs.Add(Array.Empty<byte>());
                    continue;
                }

                if (reader.BaseStream.Position + length > end)
                    break;

                var data = reader.ReadBytes((int)length);
                blobs.Add(data);
            }
        }

        public byte[] Get(uint index)
        {
            if (index == 0) return Array.Empty<byte>();
            if (index >= blobs.Count) return Array.Empty<byte>();
            return blobs[(int)index] ?? Array.Empty<byte>();
        }

        private static uint ReadCompressedUInt32(BinaryReader reader)
        {
            var b1 = reader.ReadByte();
            if ((b1 & 0x80) == 0)
                return b1;
            if ((b1 & 0x40) == 0)
            {
                var b2 = reader.ReadByte();
                return (uint)(((b1 & 0x3F) << 8) | b2);
            }
            var b3 = reader.ReadByte();
            var b4 = reader.ReadByte();
            var b5 = reader.ReadByte();
            return (uint)(((b1 & 0x1F) << 24) | (b3 << 16) | (b4 << 8) | b5);
        }

        public override string ToString() => $"#Blob ({Count} blobs)";
    }
}