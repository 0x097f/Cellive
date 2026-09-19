namespace CilDotNet.Cil.Metadata
{
    public sealed class UserStringHeap
    {
        private readonly List<string> strings = new() { string.Empty };

        public IReadOnlyList<string> Strings => strings;
        public int Count => strings.Count;

        public void Read(BinaryReader reader, uint offset, uint size)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            var end = offset + size;
            while (reader.BaseStream.Position < end)
            {
                var length = ReadCompressedUInt32(reader);
                if (length == 0)
                {
                    strings.Add(string.Empty);
                    continue;
                }

                if (reader.BaseStream.Position + length > end)
                    break;

                var data = reader.ReadBytes((int)length);
                if (data.Length > 0 && data[data.Length - 1] == 1)
                {
                    var trimmed = new byte[data.Length - 1];
                    Array.Copy(data, trimmed, trimmed.Length);
                    data = trimmed;
                }

                strings.Add(Encoding.Unicode.GetString(data));
            }
        }

        public string Get(uint index)
        {
            if (index == 0) return string.Empty;
            if (index >= strings.Count) return string.Empty;
            return strings[(int)index] ?? string.Empty;
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

        public override string ToString() => $"#US ({Count} strings)";
    }
}