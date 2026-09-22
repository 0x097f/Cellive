namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ParamRow
    {
        public ushort Flags { get; set; }
        public ushort Sequence { get; set; }
        public uint NameIndex { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsIn => (Flags & 0x0001) != 0;
        public bool IsOut => (Flags & 0x0002) != 0;
        public bool IsLcid => (Flags & 0x0004) != 0;
        public bool IsRetval => (Flags & 0x0008) != 0;
        public bool IsOptional => (Flags & 0x0010) != 0;
        public bool HasDefault => (Flags & 0x1000) != 0;
        public bool HasFieldMarshal => (Flags & 0x2000) != 0;

        public override string ToString() => Name;
    }
}