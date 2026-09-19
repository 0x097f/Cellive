namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MemberRefRow
    {
        public uint Class { get; set; }
        public uint NameIndex { get; set; }
        public uint SignatureIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();
        public MetadataToken ClassToken { get; set; }

        public override string ToString() => Name;
    }
}