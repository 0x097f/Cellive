namespace Cellive.Cil.Metadata.Tables.Enc
{
    public sealed class EncLogRow
    {
        public uint Token { get; set; }
        public uint FuncCode { get; set; }

        public MetadataToken TokenValue { get; set; }

        public override string ToString() =>
            $"EncLog Token=0x{Token:X8} FuncCode=0x{FuncCode:X8}";
    }
}