namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldRVATable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<FieldRVARow> rows = new();

        public IReadOnlyList<FieldRVARow> Rows => rows;
        public int Count => rows.Count;

        public FieldRVATable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.FieldRVA);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.FieldRVA);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new FieldRVARow
                {
                    RVA = reader.ReadUInt32(),
                    Field = reader.ReadTableIndex(TableType.Field),
                };

                row.FieldToken = MetadataToken.FromToken(TableType.Field, row.Field);
                rows.Add(row);
            }
        }

        public FieldRVARow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public FieldRVARow? FindByField(MetadataToken field)
        {
            foreach (var row in rows)
            {
                if (row.FieldToken == field)
                    return row;
            }
            return null;
        }
    }
}