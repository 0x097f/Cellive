namespace Cellive.Cil.Metadata.Tables
{
    public sealed class GenericParamRow
    {
        public ushort Number { get; set; }
        public ushort Flags { get; set; }
        public uint Owner { get; set; }
        public uint NameIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public MetadataToken OwnerToken { get; set; }

        public bool IsVarianceMask => (Flags & 0x0003) != 0;
        public bool IsCovariant => (Flags & 0x0003) == 0x0001;
        public bool IsContravariant => (Flags & 0x0003) == 0x0002;
        public bool IsReferenceTypeConstraint => (Flags & 0x0004) != 0;
        public bool IsNotNullableValueTypeConstraint => (Flags & 0x0008) != 0;
        public bool IsDefaultConstructorConstraint => (Flags & 0x0010) != 0;

        public override string ToString() => Name;
    }
}