using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public interface IPropertyDef
    {
        MetadataToken Token { get; }
        string Name { get; }

        ITypeDef? DeclaringType { get; }
        ITypeDef? PropertyType { get; }

        ushort Flags { get; }
        byte[] Signature { get; }

        bool IsSpecialName { get; }
        bool IsRTSpecialName { get; }

        IMethodDef? Getter { get; }
        IMethodDef? Setter { get; }
        IMethodDef? OtherMethod { get; }

        bool HasGetter { get; }
        bool HasSetter { get; }

        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}