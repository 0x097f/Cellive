namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class TypeDefRow
    {
        public uint Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint NamespaceIndex { get; set; }
        public uint Extends { get; set; }
        public uint FieldList { get; set; }
        public uint MethodList { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public MetadataToken ExtendsToken { get; set; }
        public MetadataToken FieldListToken { get; set; }
        public MetadataToken MethodListToken { get; set; }

        public override string ToString() =>
            string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";
    }
}