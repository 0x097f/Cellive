namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class CustomAttributeRow
    {
        public uint Parent { get; set; }
        public uint Type { get; set; }
        public uint ValueIndex { get; set; }

        public byte[] Value { get; set; } = System.Array.Empty<byte>();
        public MetadataToken ParentToken { get; set; }
        public MetadataToken TypeToken { get; set; }

        public override string ToString() =>
            $"CustomAttribute Parent={ParentToken} Type={TypeToken} ({Value.Length} bytes)";
    }
}