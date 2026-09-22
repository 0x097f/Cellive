namespace Cellive.Cil.Metadata.Tables
{
    public sealed class TypeDefTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<TypeDefRow> rows = new();

        public IReadOnlyList<TypeDefRow> Rows => rows;
        public int Count => rows.Count;

        public TypeDefTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.TypeDef);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.TypeDef);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new TypeDefRow
                {
                    Flags = reader.ReadUInt32(),
                    NameIndex = reader.ReadStringIndex(),
                    NamespaceIndex = reader.ReadStringIndex(),
                    Extends = reader.ReadCodedIndex(CodedIndex.TypeDefOrRef).Value,
                    FieldList = reader.ReadTableIndex(TableType.Field),
                    MethodList = reader.ReadTableIndex(TableType.Method),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Namespace = metadata.Strings.Get(row.NamespaceIndex);
                row.ExtendsToken = new MetadataToken(row.Extends);
                row.FieldListToken = MetadataToken.FromToken(TableType.Field, row.FieldList);
                row.MethodListToken = MetadataToken.FromToken(TableType.Method, row.MethodList);

                rows.Add(row);
            }
        }

        public TypeDefRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public TypeDefRow? FindByName(string name)
        {
            foreach (var row in rows)
            {
                if (row.Name == name)
                    return row;
            }
            return null;
        }
    }
}
