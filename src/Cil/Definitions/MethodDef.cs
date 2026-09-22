using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class MethodDef : IMethodDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName => $"{DeclaringType?.FullName ?? "?"}.{Name}";

        public ITypeDef? DeclaringType { get; set; }
        public ITypeDef? ReturnType { get; set; }

        public ushort Flags { get; set; }
        public ushort ImplFlags { get; set; }
        public uint RVA { get; set; }
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public bool IsStatic => (Flags & 0x0010) != 0;
        public bool IsPublic => (Flags & 0x0007) == 0x0006;
        public bool IsPrivate => (Flags & 0x0007) == 0x0001;
        public bool IsVirtual => (Flags & 0x0040) != 0;
        public bool IsAbstract => (Flags & 0x0400) != 0;
        public bool IsFinal => (Flags & 0x0020) != 0;
        public bool IsConstructor => Name == ".ctor";
        public bool IsStaticConstructor => Name == ".cctor";
        public bool IsGenericMethod => GenericParametersList.Count > 0;
        public bool HasBody => Body != null;

        public List<IParamDef> ParametersList { get; } = new();
        public List<IGenericParam> GenericParametersList { get; } = new();
        public List<ICustomAttribute> CustomAttributesList { get; } = new();

        public IReadOnlyList<IParamDef> Parameters => ParametersList;
        public IReadOnlyList<IGenericParam> GenericParameters => GenericParametersList;
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public IMethodBody? Body { get; set; }

        public override string ToString() => FullName;
    }
}