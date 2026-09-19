namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class TypeRefTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<TypeRefRow> rows = new();

        public IReadOnlyList<TypeRefRow> Rows => rows;
        public int Count => rows.Count;

        public TypeRefTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.TypeRef);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.TypeRef);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new TypeRefRow
                {
                    ResolutionScope = reader.ReadCodedIndex(CodedIndex.ResolutionScope).Value,
                    NameIndex = reader.ReadStringIndex(),
                    NamespaceIndex = reader.ReadStringIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Namespace = metadata.Strings.Get(row.NamespaceIndex);
                row.ResolutionScopeToken = new MetadataToken(row.ResolutionScope);
                rows.Add(row);
            }
        }

        public TypeRefRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public TypeRefRow? FindByName(string name)
        {
            foreach (var row in rows)
            {
                if (row.Name == name)
                    return row;
            }
            return null;
        }

        public TypeRefRow? FindByFullName(string fullName)
        {
            foreach (var row in rows)
            {
                if (row.FullName == fullName)
                    return row;
            }
            return null;
        }
    }
}