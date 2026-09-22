using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class ParamDef : IParamDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Index { get; set; }
        public ushort Sequence { get; set; }

        public IMethodDef? Method { get; set; }
        public ITypeDef? ParameterType { get; set; }

        public ushort Flags { get; set; }

        public bool IsIn => (Flags & 0x0001) != 0;
        public bool IsOut => (Flags & 0x0002) != 0;
        public bool IsOptional => (Flags & 0x0010) != 0;
        public bool HasDefault => (Flags & 0x1000) != 0;

        public object? DefaultValue { get; set; }

        public List<ICustomAttribute> CustomAttributesList { get; } = new();
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() =>
            string.IsNullOrEmpty(Name) ? $"p{Index}" : Name;
    }
}