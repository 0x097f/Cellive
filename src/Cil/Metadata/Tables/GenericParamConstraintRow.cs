namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class GenericParamConstraintRow
    {
        public uint Owner { get; set; }
        public uint Constraint { get; set; }

        public MetadataToken OwnerToken { get; set; }
        public MetadataToken ConstraintToken { get; set; }

        public override string ToString() =>
            $"GenericParamConstraint {OwnerToken} -> {ConstraintToken}";
    }
}