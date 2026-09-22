namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldRow
    {
        public ushort Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint SignatureIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public bool IsStatic => (Flags & 0x0010) != 0;
        public bool IsPublic => (Flags & 0x0007) == 0x0006;
        public bool IsPrivate => (Flags & 0x0007) == 0x0001;
        public bool IsInitOnly => (Flags & 0x0020) != 0;
        public bool IsLiteral => (Flags & 0x0040) != 0;

        public override string ToString() => Name;
    }
}