namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class StandAloneSigRow
    {
        public uint SignatureIndex { get; set; }
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public override string ToString() => $"StandAloneSig ({Signature.Length} bytes)";
    }
}