namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldLayoutTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<FieldLayoutRow> rows = new();

        public IReadOnlyList<FieldLayoutRow> Rows => rows;
        public int Count => rows.Count;

        public FieldLayoutTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.FieldLayout);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.FieldLayout);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new FieldLayoutRow
                {
                    Offset = reader.ReadUInt32(),
                    Field = reader.ReadTableIndex(TableType.Field),
                };

                row.FieldToken = MetadataToken.FromToken(TableType.Field, row.Field);
                rows.Add(row);
            }
        }

        public FieldLayoutRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public FieldLayoutRow? FindByField(MetadataToken field)
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