namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FileRow
    {
        public uint Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint HashValueIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte[] HashValue { get; set; } = System.Array.Empty<byte>();

        public bool ContainsMetadata => (Flags & 0x0000) == 0;
        public bool ContainsNoMetadata => (Flags & 0x0001) != 0;

        public override string ToString() => Name;
    }
}