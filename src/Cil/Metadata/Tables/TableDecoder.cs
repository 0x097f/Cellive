namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class TableDecoder
    {
        private readonly MetadataReader metadata;
        private readonly Dictionary<TableType, int[]> rowSizes = new();

        public TableDecoder(MetadataReader metadata)
        {
            this.metadata = metadata;
        }

        public int GetRowSize(TableType table)
        {
            if (rowSizes.TryGetValue(table, out var size))
                return size[0];

            ComputeRowSize(table);
            return rowSizes.TryGetValue(table, out var computed) ? computed[0] : 0;
        }

        private void ComputeRowSize(TableType table)
        {
            //Ecma standard
            int size = table switch
            {
                TableType.Module => 2 + GetStringIndexSize() + 3 * GetGuidIndexSize(),
                TableType.TypeRef => GetStringIndexSize() + GetStringIndexSize() + GetCodedIndexSize(CodedIndex.ResolutionScope),
                TableType.TypeDef => 4 + GetStringIndexSize() + GetStringIndexSize() + GetCodedIndexSize(CodedIndex.TypeDefOrRef) + GetTableIndexSize(TableType.Field) + GetTableIndexSize(TableType.Method),
                TableType.Field => 2 + GetStringIndexSize() + GetBlobIndexSize(),
                TableType.Method => 4 + 2 + 2 + GetStringIndexSize() + GetBlobIndexSize() + GetTableIndexSize(TableType.Param),
                TableType.Param => 2 + 2 + GetStringIndexSize(),
                TableType.InterfaceImpl => GetTableIndexSize(TableType.TypeDef) + GetCodedIndexSize(CodedIndex.TypeDefOrRef),
                TableType.MemberRef => GetCodedIndexSize(CodedIndex.MemberRefParent) + GetStringIndexSize() + GetBlobIndexSize(),
                TableType.Constant => 1 + 1 + GetCodedIndexSize(CodedIndex.HasConstant) + GetBlobIndexSize(),
                TableType.CustomAttribute => GetCodedIndexSize(CodedIndex.HasCustomAttribute) + GetCodedIndexSize(CodedIndex.CustomAttributeType) + GetBlobIndexSize(),
                TableType.FieldMarshal => GetCodedIndexSize(CodedIndex.HasFieldMarshal) + GetBlobIndexSize(),
                TableType.DeclSecurity => 2 + GetCodedIndexSize(CodedIndex.HasDeclSecurity) + GetBlobIndexSize(),
                TableType.ClassLayout => 2 + 4 + GetTableIndexSize(TableType.TypeDef),
                TableType.FieldLayout => 4 + GetTableIndexSize(TableType.Field),
                TableType.StandAloneSig => GetBlobIndexSize(),
                TableType.EventMap => GetTableIndexSize(TableType.TypeDef) + GetTableIndexSize(TableType.Event),
                TableType.Event => 2 + GetStringIndexSize() + GetCodedIndexSize(CodedIndex.TypeDefOrRef),
                TableType.PropertyMap => GetTableIndexSize(TableType.TypeDef) + GetTableIndexSize(TableType.Property),
                TableType.Property => 2 + GetStringIndexSize() + GetBlobIndexSize(),
                TableType.MethodSemantics => 2 + GetTableIndexSize(TableType.Method) + GetCodedIndexSize(CodedIndex.HasSemantics),
                TableType.MethodImpl => GetTableIndexSize(TableType.TypeDef) + GetCodedIndexSize(CodedIndex.MethodDefOrRef) + GetCodedIndexSize(CodedIndex.MethodDefOrRef),
                TableType.ModuleRef => GetStringIndexSize(),
                TableType.TypeSpec => GetBlobIndexSize(),
                TableType.ImplMap => 2 + GetCodedIndexSize(CodedIndex.MemberForwarded) + GetStringIndexSize() + GetTableIndexSize(TableType.ModuleRef),
                TableType.FieldRVA => 4 + GetTableIndexSize(TableType.Field),
                TableType.Assembly => 4 + 2 + 2 + 2 + 2 + 4 + GetBlobIndexSize() + GetStringIndexSize() + GetStringIndexSize(),
                TableType.AssemblyRef => 2 + 2 + 2 + 2 + 4 + GetBlobIndexSize() + GetStringIndexSize() + GetStringIndexSize() + GetBlobIndexSize(),
                TableType.File => 4 + GetStringIndexSize() + GetBlobIndexSize(),
                TableType.ExportedType => 4 + 4 + GetStringIndexSize() + GetStringIndexSize() + GetCodedIndexSize(CodedIndex.Implementation),
                TableType.ManifestResource => 4 + 4 + GetStringIndexSize() + GetCodedIndexSize(CodedIndex.Implementation),
                TableType.NestedClass => GetTableIndexSize(TableType.TypeDef) + GetTableIndexSize(TableType.TypeDef),
                TableType.GenericParam => 2 + 2 + GetCodedIndexSize(CodedIndex.TypeOrMethodDef) + GetStringIndexSize(),
                TableType.MethodSpec => GetCodedIndexSize(CodedIndex.MethodDefOrRef) + GetBlobIndexSize(),
                TableType.GenericParamConstraint => GetTableIndexSize(TableType.GenericParam) + GetCodedIndexSize(CodedIndex.TypeDefOrRef),
                _ => 0,
            };

            rowSizes[table] = new[] { size };
        }

        private int GetStringIndexSize()
            => metadata.IsStringHeapLarge ? 4 : 2;

        private int GetGuidIndexSize()
            => metadata.IsGuidHeapLarge ? 4 : 2;

        private int GetBlobIndexSize()
            => metadata.IsBlobHeapLarge ? 4 : 2;

        private int GetTableIndexSize(TableType table)
        {
            var rows = metadata.GetRowCount(table);
            return rows > 0xFFFF ? 4 : 2;
        }

        private int GetCodedIndexSize(CodedIndex index)
        {
            return CodedIndexReader.GetSize(index, metadata.Tables);
        }
    }
}