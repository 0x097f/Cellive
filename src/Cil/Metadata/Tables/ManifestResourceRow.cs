namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ManifestResourceRow
    {
        public uint Offset { get; set; }
        public uint Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint Implementation { get; set; }

        public string Name { get; set; } = string.Empty;
        public MetadataToken ImplementationToken { get; set; }

        public bool IsPublic => (Flags & 0x0001) != 0;
        public bool IsPrivate => (Flags & 0x0002) != 0;
        public bool IsEmbedded => Implementation == 0;

        public override string ToString() => Name;
    }
}