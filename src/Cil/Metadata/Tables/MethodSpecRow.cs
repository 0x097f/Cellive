namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MethodSpecRow
    {
        public uint Method { get; set; }
        public uint InstantiationIndex { get; set; }

        public byte[] Instantiation { get; set; } = System.Array.Empty<byte>();
        public MetadataToken MethodToken { get; set; }

        public override string ToString() =>
            $"MethodSpec {MethodToken} ({Instantiation.Length} bytes)";
    }
}