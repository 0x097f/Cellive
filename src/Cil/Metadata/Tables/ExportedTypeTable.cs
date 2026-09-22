namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ExportedTypeTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ExportedTypeRow> rows = new();

        public IReadOnlyList<ExportedTypeRow> Rows => rows;
        public int Count => rows.Count;

        public ExportedTypeTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ExportedType);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ExportedType);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ExportedTypeRow
                {
                    Flags = reader.ReadUInt32(),
                    TypeDefId = reader.ReadUInt32(),
                    NameIndex = reader.ReadStringIndex(),
                    NamespaceIndex = reader.ReadStringIndex(),
                    Implementation = reader.ReadCodedIndex(CodedIndex.Implementation).Value,
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Namespace = metadata.Strings.Get(row.NamespaceIndex);
                row.ImplementationToken = new MetadataToken(row.Implementation);

                rows.Add(row);
            }
        }

        public ExportedTypeRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ExportedTypeRow? FindByFullName(string fullName)
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