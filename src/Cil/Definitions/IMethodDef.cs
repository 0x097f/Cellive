using Cellive.Cil.Metadata;
using static System.Reflection.Metadata.Ecma335.MethodBodyStreamEncoder;

namespace Cellive.Cil.Definitions
{
    public interface IMethodDef
    {
        MetadataToken Token { get; }
        string Name { get; }
        string FullName { get; }

        ITypeDef? DeclaringType { get; }
        ITypeDef? ReturnType { get; }

        ushort Flags { get; }
        ushort ImplFlags { get; }
        uint RVA { get; }
        byte[] Signature { get; }

        bool IsStatic { get; }
        bool IsPublic { get; }
        bool IsPrivate { get; }
        bool IsVirtual { get; }
        bool IsAbstract { get; }
        bool IsFinal { get; }
        bool IsConstructor { get; }
        bool IsStaticConstructor { get; }
        bool IsGenericMethod { get; }
        bool HasBody { get; }

        IReadOnlyList<IParamDef> Parameters { get; }
        IReadOnlyList<IGenericParam> GenericParameters { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }

        IMethodBody? Body { get; }
    }
}