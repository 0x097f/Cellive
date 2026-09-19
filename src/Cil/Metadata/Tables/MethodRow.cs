namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MethodRow
    {
        public uint RVA { get; set; }
        public ushort ImplFlags { get; set; }
        public ushort Flags { get; set; }
        public uint NameIndex { get; set; }
        public uint SignatureIndex { get; set; }
        public uint ParamList { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();
        public MetadataToken ParamListToken { get; set; }

        public bool HasBody => RVA != 0;

        public override string ToString() => Name;
    }
}