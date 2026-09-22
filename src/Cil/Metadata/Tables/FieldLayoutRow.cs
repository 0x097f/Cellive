namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldLayoutRow
    {
        public uint Offset { get; set; }
        public uint Field { get; set; }

        public MetadataToken FieldToken { get; set; }

        public override string ToString() => $"FieldLayout {FieldToken} @ 0x{Offset:X}";
    }
}