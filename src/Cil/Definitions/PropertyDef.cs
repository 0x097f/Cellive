using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class PropertyDef : IPropertyDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;

        public ITypeDef? DeclaringType { get; set; }
        public ITypeDef? PropertyType { get; set; }

        public ushort Flags { get; set; }
        public byte[] Signature { get; set; } = System.Array.Empty<byte>();

        public bool IsSpecialName => (Flags & 0x0200) != 0;
        public bool IsRTSpecialName => (Flags & 0x0400) != 0;

        public IMethodDef? Getter { get; set; }
        public IMethodDef? Setter { get; set; }
        public IMethodDef? OtherMethod { get; set; }

        public bool HasGetter => Getter != null;
        public bool HasSetter => Setter != null;

        public List<ICustomAttribute> CustomAttributesList { get; } = new();
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() => Name;
    }
}