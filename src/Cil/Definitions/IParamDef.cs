using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public interface IParamDef
    {
        MetadataToken Token { get; }
        string Name { get; }
        int Index { get; }
        ushort Sequence { get; }

        IMethodDef? Method { get; }
        ITypeDef? ParameterType { get; }

        ushort Flags { get; }

        bool IsIn { get; }
        bool IsOut { get; }
        bool IsOptional { get; }
        bool HasDefault { get; }

        object? DefaultValue { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}