namespace CilDotNet.Cil.Metadata
{
    public sealed class GuidHeap
    {
        private readonly List<Guid> guids = new();

        public IReadOnlyList<Guid> Guids => guids;
        public int Count => guids.Count;

        public void Read(BinaryReader reader, uint offset, uint size)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            var end = offset + size;
            while (reader.BaseStream.Position + 16 <= end)
            {
                var bytes = reader.ReadBytes(16);
                guids.Add(new Guid(bytes));
            }
        }

        public Guid Get(uint index)
        {
            if (index == 0) return Guid.Empty;
            if (index > guids.Count) return Guid.Empty;
            return guids[(int)index - 1];
        }

        public override string ToString() => $"#GUID ({Count} guids)";
    }
}