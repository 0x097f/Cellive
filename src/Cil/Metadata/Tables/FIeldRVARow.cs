namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldRVARow
    {
        public uint RVA { get; set; }
        public uint Field { get; set; }

        public MetadataToken FieldToken { get; set; }

        public override string ToString() => $"FieldRVA {FieldToken} @ 0x{RVA:X8}";
    }
}