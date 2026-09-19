using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public interface IEventDef
    {
        MetadataToken Token { get; }
        string Name { get; }

        ITypeDef? DeclaringType { get; }
        ITypeDef? EventType { get; }

        ushort Flags { get; }

        bool IsSpecialName { get; }
        bool IsRTSpecialName { get; }

        IMethodDef? AddMethod { get; }
        IMethodDef? RemoveMethod { get; }
        IMethodDef? RaiseMethod { get; }

        bool HasAddMethod { get; }
        bool HasRemoveMethod { get; }
        bool HasRaiseMethod { get; }

        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}