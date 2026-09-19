namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class PropertyMapRow
    {
        public uint Parent { get; set; }
        public uint PropertyList { get; set; }

        public MetadataToken ParentToken { get; set; }
        public MetadataToken PropertyListToken { get; set; }

        public override string ToString() =>
            $"PropertyMap {ParentToken} -> {PropertyListToken}";
    }
}