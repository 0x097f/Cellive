namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class NestedClassRow
    {
        public uint NestedClass { get; set; }
        public uint EnclosingClass { get; set; }

        public MetadataToken NestedClassToken { get; set; }
        public MetadataToken EnclosingClassToken { get; set; }

        public override string ToString() =>
            $"NestedClass {NestedClassToken} in {EnclosingClassToken}";
    }
}