namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MethodSemanticsRow
    {
        public ushort Semantics { get; set; }
        public uint Method { get; set; }
        public uint Association { get; set; }

        public MetadataToken MethodToken { get; set; }
        public MetadataToken AssociationToken { get; set; }

        public bool IsSetter => (Semantics & 0x0001) != 0;
        public bool IsGetter => (Semantics & 0x0002) != 0;
        public bool IsOther => (Semantics & 0x0004) != 0;
        public bool IsAddOn => (Semantics & 0x0008) != 0;
        public bool IsRemoveOn => (Semantics & 0x0010) != 0;
        public bool IsFire => (Semantics & 0x0020) != 0;

        public override string ToString() =>
            $"MethodSemantics {MethodToken} -> {AssociationToken} (0x{Semantics:X4})";
    }
}