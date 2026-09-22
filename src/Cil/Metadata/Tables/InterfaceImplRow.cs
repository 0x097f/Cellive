namespace Cellive.Cil.Metadata.Tables
{
    public sealed class InterfaceImplRow
    {
        public uint Class { get; set; }
        public uint Interface { get; set; }

        public MetadataToken ClassToken { get; set; }
        public MetadataToken InterfaceToken { get; set; }

        public override string ToString() =>
            $"InterfaceImpl Class={ClassToken} Interface={InterfaceToken}";
    }
}