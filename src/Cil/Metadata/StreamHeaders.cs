namespace CilDotNet.Cil.Metadata
{
    public sealed class StreamHeaders
    {
        public IReadOnlyList<StreamHeader> Streams => streams;

        private readonly List<StreamHeader> streams = new();

        public static StreamHeaders Read(BinaryReader reader, MetadataHeader header)
        {
            var result = new StreamHeaders();

            for (int i = 0; i < header.StreamCount; i++)
            {
                result.streams.Add(StreamHeader.Read(reader));
            }

            return result;
        }

        public StreamHeader? GetStream(string name)
        {
            foreach (var stream in streams)
            {
                if (stream.Name == name)
                    return stream;
            }
            return null;
        }

        public bool HasStream(string name) => GetStream(name) != null;

        public override string ToString() => string.Join(", ", streams);
    }

    public sealed class StreamHeader
    {
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public string Name { get; set; } = string.Empty;

        public static StreamHeader Read(BinaryReader reader)
        {
            var header = new StreamHeader
            {
                Offset = reader.ReadUInt32(),
                Size = reader.ReadUInt32(),
            };

            var nameBuilder = new StringBuilder();
            while (true)
            {
                var b = reader.ReadByte();
                if (b == 0) break;
                nameBuilder.Append((char)b);
            }

            header.Name = nameBuilder.ToString();
            var pos = reader.BaseStream.Position;
            var mod = pos % 4;
            if (mod != 0)
                reader.BaseStream.Seek(4 - mod, SeekOrigin.Current);

            return header;
        }

        public override string ToString() => $"{Name} (0x{Offset:X}, {Size} bytes)";
    }
}