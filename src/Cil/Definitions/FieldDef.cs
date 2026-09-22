using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class FieldDef : IFieldDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;

        public ITypeDef? DeclaringType { get; set; }
        public ITypeDef? FieldType { get; set; }

        public ushort Flags { get; set; }
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public bool IsStatic => (Flags & 0x0010) != 0;
        public bool IsPublic => (Flags & 0x0007) == 0x0006;
        public bool IsPrivate => (Flags & 0x0007) == 0x0001;
        public bool IsInitOnly => (Flags & 0x0020) != 0;
        public bool IsLiteral => (Flags & 0x0040) != 0;
        public bool HasConstant => ConstantValue != null;

        public object? ConstantValue { get; set; }

        public List<ICustomAttribute> CustomAttributesList { get; } = new();
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() => Name;
    }
}