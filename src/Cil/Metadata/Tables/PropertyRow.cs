namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class PropertyRow
    {
        public ushort Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint TypeIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte[] Type { get; set; } = System.Array.Empty<byte>();

        public bool IsSpecialName => (Flags & 0x0200) != 0;
        public bool IsRTSpecialName => (Flags & 0x0400) != 0;
        public bool HasDefault => (Flags & 0x1000) != 0;

        public override string ToString() => Name;
    }
}