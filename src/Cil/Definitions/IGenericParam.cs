using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public interface IGenericParam
    {
        MetadataToken Token { get; }
        string Name { get; }
        int Position { get; }
        ushort Flags { get; }

        ITypeDef? OwnerType { get; }
        IMethodDef? OwnerMethod { get; }

        bool IsCovariant { get; }
        bool IsContravariant { get; }
        bool HasReferenceTypeConstraint { get; }
        bool HasNotNullableValueTypeConstraint { get; }
        bool HasDefaultConstructorConstraint { get; }

        IReadOnlyList<ITypeDef> Constraints { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}