using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class TypeRef : ITypeDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;

        public string FullName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";
        public string AssemblyQualifiedName => FullName;

        public uint Flags => 0;
        public ITypeDef? BaseType => null;
        public ITypeDef? DeclaringType => null;
        public IModuleDef Module => null!;
        public IAssemblyDef Assembly => null!;

        public bool IsPublic => true;
        public bool IsNested => false;
        public bool IsClass => true;
        public bool IsInterface => false;
        public bool IsEnum => false;
        public bool IsValueType => false;
        public bool IsAbstract => false;
        public bool IsSealed => false;
        public bool IsGenericType => false;
        public bool IsGenericInstance => false;

        public IReadOnlyList<IGenericParam> GenericParameters => System.Array.Empty<IGenericParam>();
        public IReadOnlyList<ITypeDef> Interfaces => System.Array.Empty<ITypeDef>();
        public IReadOnlyList<IFieldDef> Fields => System.Array.Empty<IFieldDef>();
        public IReadOnlyList<IMethodDef> Methods => System.Array.Empty<IMethodDef>();
        public IReadOnlyList<IPropertyDef> Properties => System.Array.Empty<IPropertyDef>();
        public IReadOnlyList<IEventDef> Events => System.Array.Empty<IEventDef>();
        public IReadOnlyList<ITypeDef> NestedTypes => System.Array.Empty<ITypeDef>();
        public IReadOnlyList<ICustomAttribute> CustomAttributes => System.Array.Empty<ICustomAttribute>();

        public IFieldDef? GetField(string name) => null;
        public IMethodDef? GetMethod(string name) => null;
        public ITypeDef? GetNestedType(string name) => null;
        public bool IsAssignableTo(ITypeDef other) => FullName == other?.FullName;

        public override string ToString() => FullName;
    }
}
