using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public sealed class EventDef : IEventDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;

        public ITypeDef? DeclaringType { get; set; }
        public ITypeDef? EventType { get; set; }

        public ushort Flags { get; set; }

        public bool IsSpecialName => (Flags & 0x0200) != 0;
        public bool IsRTSpecialName => (Flags & 0x0400) != 0;

        public IMethodDef? AddMethod { get; set; }
        public IMethodDef? RemoveMethod { get; set; }
        public IMethodDef? RaiseMethod { get; set; }

        public bool HasAddMethod => AddMethod != null;
        public bool HasRemoveMethod => RemoveMethod != null;
        public bool HasRaiseMethod => RaiseMethod != null;

        public List<ICustomAttribute> CustomAttributesList { get; } = new();
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() => Name;
    }
}