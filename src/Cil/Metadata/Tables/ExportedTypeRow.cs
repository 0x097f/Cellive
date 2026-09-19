namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ExportedTypeRow
    {
        public uint Flags { get; set; }
        public uint TypeDefId { get; set; }
        public uint NameIndex { get; set; }
        public uint NamespaceIndex { get; set; }
        public uint Implementation { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public MetadataToken ImplementationToken { get; set; }

        public bool IsPublic => (Flags & 0x00000001) != 0;
        public bool IsNested => (Flags & 0x00000002) != 0;
        public bool IsForwarder => (Flags & 0x00200000) != 0;

        public string FullName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

        public override string ToString() => FullName;
    }
}