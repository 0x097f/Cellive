using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public interface ITypeDef
    {
        MetadataToken Token { get; }
        string Name { get; }
        string Namespace { get; }
        string FullName { get; }
        string AssemblyQualifiedName { get; }

        uint Flags { get; }
        ITypeDef? BaseType { get; }
        ITypeDef? DeclaringType { get; }
        IModuleDef Module { get; }
        IAssemblyDef Assembly { get; }

        bool IsPublic { get; }
        bool IsNested { get; }
        bool IsClass { get; }
        bool IsInterface { get; }
        bool IsEnum { get; }
        bool IsValueType { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
        bool IsGenericType { get; }
        bool IsGenericInstance { get; }

        IReadOnlyList<IGenericParam> GenericParameters { get; }
        IReadOnlyList<ITypeDef> Interfaces { get; }
        IReadOnlyList<IFieldDef> Fields { get; }
        IReadOnlyList<IMethodDef> Methods { get; }
        IReadOnlyList<IPropertyDef> Properties { get; }
        IReadOnlyList<IEventDef> Events { get; }
        IReadOnlyList<ITypeDef> NestedTypes { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }

        IFieldDef? GetField(string name);
        IMethodDef? GetMethod(string name);
        ITypeDef? GetNestedType(string name);
        bool IsAssignableTo(ITypeDef other);
    }
}