namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class EventRow
    {
        public ushort Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint EventType { get; set; }

        public string Name { get; set; } = string.Empty;
        public MetadataToken EventTypeToken { get; set; }

        public bool IsSpecialName => (Flags & 0x0200) != 0;
        public bool IsRTSpecialName => (Flags & 0x0400) != 0;

        public override string ToString() => Name;
    }
}