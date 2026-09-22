namespace Cellive.Cil.Metadata.Tables
{
    public enum CodedIndex
    {
        TypeDefOrRef,
        HasConstant,
        HasCustomAttribute,
        HasFieldMarshal,
        HasDeclSecurity,
        MemberRefParent,
        HasSemantics,
        MethodDefOrRef,
        MemberForwarded,
        Implementation,
        CustomAttributeType,
        ResolutionScope,
        TypeOrMethodDef,
    }

    public static class CodedIndexTables
    {
        public static readonly TableType[][] Tables = new[]
        {
            //TypeDefOrRef
            new[] { TableType.TypeDef, TableType.TypeRef, TableType.TypeSpec },
            //HasConstant
            new[] { TableType.Field, TableType.Param, TableType.Property },
            //HasCustomAttribute
            new[]
            {
                TableType.Method, TableType.Field, TableType.TypeRef, TableType.TypeDef,
                TableType.Param, TableType.InterfaceImpl, TableType.MemberRef,
                TableType.Module, TableType.DeclSecurity, TableType.Property,
                TableType.Event, TableType.StandAloneSig, TableType.ModuleRef,
                TableType.TypeSpec, TableType.Assembly, TableType.AssemblyRef,
                TableType.File, TableType.ExportedType, TableType.ManifestResource,
                TableType.GenericParam, TableType.GenericParamConstraint,
                TableType.MethodSpec,
            },
            //HasFieldMarshal
            new[] { TableType.Field, TableType.Param },
            //HasDeclSecurity
            new[] { TableType.TypeDef, TableType.Method, TableType.Assembly },
            //MemberRefParent
            new[] { TableType.TypeDef, TableType.TypeRef, TableType.ModuleRef, TableType.Method, TableType.TypeSpec },
            //HasSemantics
            new[] { TableType.Event, TableType.Property },
            //MethodDefOrRef
            new[] { TableType.Method, TableType.MemberRef },
            //MemberForwarded
            new[] { TableType.Field, TableType.Method },
            //Implementation
            new[] { TableType.File, TableType.AssemblyRef, TableType.ExportedType },
            //CustomAttributeType
            new[] { TableType.NotUsed, TableType.NotUsed, TableType.Method, TableType.MemberRef, TableType.NotUsed },
            //ResolutionScope
            new[] { TableType.Module, TableType.ModuleRef, TableType.AssemblyRef, TableType.TypeRef },
            //TypeOrMethodDef
            new[] { TableType.TypeDef, TableType.Method },
        };
        public static readonly int[] TagBits = new[]
        {
            2,  //TypeDefOrRef
            2,  //HasConstant
            5,  //HasCustomAttribute
            1,  //HasFieldMarshal
            2,  //HasDeclSecurity
            3,  //MemberRefParent
            1,  //HasSemantics
            1,  //MethodDefOrRef
            1,  //MemberForwarded
            2,  //Implementation
            3,  //CustomAttributeType
            2,  //ResolutionScope
            1,  //TypeOrMethodDef
        };
    }
}