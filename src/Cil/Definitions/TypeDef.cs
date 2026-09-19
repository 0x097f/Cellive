using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public sealed class TypeDef : ITypeDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;

        public string FullName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";
        public string AssemblyQualifiedName => $"{FullName}, {Assembly?.Name ?? "?"}";

        public uint Flags { get; set; }
        public ITypeDef? BaseType { get; set; }
        public ITypeDef? DeclaringType { get; set; }
        public IModuleDef Module { get; set; } = null!;
        public IAssemblyDef Assembly => Module.Assembly ?? null!;

        public bool IsPublic => (Flags & 0x00000001) != 0;
        public bool IsNested => (Flags & 0x00000080) != 0;
        public bool IsInterface => (Flags & 0x00000020) != 0;
        public bool IsAbstract => (Flags & 0x00000080) != 0;
        public bool IsSealed => (Flags & 0x00000100) != 0;
        public bool IsClass => !IsInterface && !IsValueType && !IsEnum;
        public bool IsEnum => BaseType?.FullName == "System.Enum";
        public bool IsValueType => BaseType?.FullName == "System.ValueType" && !IsEnum;
        public bool IsGenericType => GenericParameters.Count > 0;
        public bool IsGenericInstance => false;

        public List<IGenericParam> GenericParametersList { get; } = new();
        public List<ITypeDef> InterfacesList { get; } = new();
        public List<IFieldDef> FieldsList { get; } = new();
        public List<IMethodDef> MethodsList { get; } = new();
        public List<IPropertyDef> PropertiesList { get; } = new();
        public List<IEventDef> EventsList { get; } = new();
        public List<ITypeDef> NestedTypesList { get; } = new();
        public List<ICustomAttribute> CustomAttributesList { get; } = new();

        public IReadOnlyList<IGenericParam> GenericParameters => GenericParametersList;
        public IReadOnlyList<ITypeDef> Interfaces => InterfacesList;
        public IReadOnlyList<IFieldDef> Fields => FieldsList;
        public IReadOnlyList<IMethodDef> Methods => MethodsList;
        public IReadOnlyList<IPropertyDef> Properties => PropertiesList;
        public IReadOnlyList<IEventDef> Events => EventsList;
        public IReadOnlyList<ITypeDef> NestedTypes => NestedTypesList;
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public IFieldDef? GetField(string name)
        {
            foreach (var field in FieldsList)
            {
                if (field.Name == name)
                    return field;
            }
            return null;
        }

        public IMethodDef? GetMethod(string name)
        {
            foreach (var method in MethodsList)
            {
                if (method.Name == name)
                    return method;
            }
            return null;
        }

        public ITypeDef? GetNestedType(string name)
        {
            foreach (var nested in NestedTypesList)
            {
                if (nested.Name == name)
                    return nested;
            }
            return null;
        }

        public bool IsAssignableTo(ITypeDef other)
        {
            if (other == null)
                return false;

            if (FullName == other.FullName)
                return true;

            var current = BaseType;
            while (current != null)
            {
                if (current.FullName == other.FullName)
                    return true;
                current = current.BaseType;
            }

            foreach (var iface in InterfacesList)
            {
                if (iface.FullName == other.FullName)
                    return true;
            }

            return false;
        }

        public override string ToString() => FullName;
    }
}