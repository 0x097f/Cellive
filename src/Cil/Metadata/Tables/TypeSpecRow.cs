namespace Cellive.Cil.Metadata.Tables
{
    public sealed class TypeSpecRow
    {
        public uint SignatureIndex { get; set; }
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public override string ToString() => $"TypeSpec ({Signature.Length} bytes)";
    }
}