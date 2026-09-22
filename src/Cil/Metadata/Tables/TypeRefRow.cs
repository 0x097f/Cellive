namespace Cellive.Cil.Metadata.Tables
{
    public sealed class TypeRefRow
    {
        public uint ResolutionScope { get; set; }
        public uint NameIndex { get; set; }
        public uint NamespaceIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public MetadataToken ResolutionScopeToken { get; set; }

        public string FullName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

        public override string ToString() => FullName;
    }
}