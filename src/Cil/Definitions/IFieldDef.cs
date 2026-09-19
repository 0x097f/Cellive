using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public interface IFieldDef
    {
        MetadataToken Token { get; }
        string Name { get; }

        ITypeDef? DeclaringType { get; }
        ITypeDef? FieldType { get; }

        ushort Flags { get; }
        byte[] Signature { get; }

        bool IsStatic { get; }
        bool IsPublic { get; }
        bool IsPrivate { get; }
        bool IsInitOnly { get; }
        bool IsLiteral { get; }
        bool HasConstant { get; }

        object? ConstantValue { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}