namespace CilDotNet.Cil.Metadata
{
    public sealed class StringHeap
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
                var sb = new StringBuilder();
                while (reader.BaseStream.Position < end)
                {
                    var b = reader.ReadByte();
                    if (b == 0) break;
                    sb.Append((char)b);
                }
                strings.Add(sb.ToString());
            }
        }

        public string Get(uint index)
        {
            if (index == 0) return string.Empty;
            if (index >= strings.Count) return string.Empty;
            return strings[(int)index] ?? string.Empty;
        }

        public override string ToString() => $"#Strings ({Count} strings)";
    }
}