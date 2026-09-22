using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class GenericParam : IGenericParam
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Position { get; set; }
        public ushort Flags { get; set; }

        public ITypeDef? OwnerType { get; set; }
        public IMethodDef? OwnerMethod { get; set; }

        public bool IsCovariant => (Flags & 0x0003) == 0x0001;
        public bool IsContravariant => (Flags & 0x0003) == 0x0002;
        public bool HasReferenceTypeConstraint => (Flags & 0x0004) != 0;
        public bool HasNotNullableValueTypeConstraint => (Flags & 0x0008) != 0;
        public bool HasDefaultConstructorConstraint => (Flags & 0x0010) != 0;

        public List<ITypeDef> ConstraintsList { get; } = new();
        public List<ICustomAttribute> CustomAttributesList { get; } = new();

        public IReadOnlyList<ITypeDef> Constraints => ConstraintsList;
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() => Name;
    }
}