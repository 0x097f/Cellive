namespace Cellive.Cil.Metadata.Tables
{
    public sealed class NestedClassTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<NestedClassRow> rows = new();

        public IReadOnlyList<NestedClassRow> Rows => rows;
        public int Count => rows.Count;

        public NestedClassTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.NestedClass);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.NestedClass);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new NestedClassRow
                {
                    NestedClass = reader.ReadTableIndex(TableType.TypeDef),
                    EnclosingClass = reader.ReadTableIndex(TableType.TypeDef),
                };

                row.NestedClassToken = MetadataToken.FromToken(TableType.TypeDef, row.NestedClass);
                row.EnclosingClassToken = MetadataToken.FromToken(TableType.TypeDef, row.EnclosingClass);

                rows.Add(row);
            }
        }

        public NestedClassRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public NestedClassRow? FindByNested(MetadataToken nestedToken)
        {
            foreach (var row in rows)
            {
                if (row.NestedClassToken == nestedToken)
                    return row;
            }
            return null;
        }

        public IEnumerable<NestedClassRow> FindByEnclosing(MetadataToken enclosingToken)
        {
            foreach (var row in rows)
            {
                if (row.EnclosingClassToken == enclosingToken)
                    yield return row;
            }
        }
    }
}