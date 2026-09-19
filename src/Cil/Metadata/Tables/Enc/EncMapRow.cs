namespace CilDotNet.Cil.Metadata.Tables.Enc
{
    public sealed class EncMapRow
    {
        public uint Token { get; set; }
        public MetadataToken TokenValue { get; set; }

        public override string ToString() => $"EncMap Token=0x{Token:X8}";
    }
}