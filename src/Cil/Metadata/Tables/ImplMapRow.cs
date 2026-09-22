namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ImplMapRow
    {
        public ushort MappingFlags { get; set; }
        public uint MemberForwarded { get; set; }
        public uint ImportNameIndex { get; set; }
        public uint ImportScope { get; set; }

        public string ImportName { get; set; } = string.Empty;
        public MetadataToken MemberForwardedToken { get; set; }
        public MetadataToken ImportScopeToken { get; set; }

        public bool IsNoMangle => (MappingFlags & 0x0001) != 0;
        public bool IsCharSetAnsi => (MappingFlags & 0x0002) != 0;
        public bool IsCharSetUnicode => (MappingFlags & 0x0004) != 0;
        public bool IsCharSetAuto => (MappingFlags & 0x0006) == 0x0006;
        public bool SupportsLastError => (MappingFlags & 0x0040) != 0;

        public override string ToString() => ImportName;
    }
}