namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ClassLayoutTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ClassLayoutRow> rows = new();

        public IReadOnlyList<ClassLayoutRow> Rows => rows;
        public int Count => rows.Count;

        public ClassLayoutTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ClassLayout);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ClassLayout);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ClassLayoutRow
                {
                    PackingSize = reader.ReadUInt16(),
                    ClassSize = reader.ReadUInt32(),
                    Parent = reader.ReadTableIndex(TableType.TypeDef),
                };

                row.ParentToken = MetadataToken.FromToken(TableType.TypeDef, row.Parent);
                rows.Add(row);
            }
        }

        public ClassLayoutRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ClassLayoutRow? FindByParent(MetadataToken parent)
        {
            foreach (var row in rows)
            {
                if (row.ParentToken == parent)
                    return row;
            }
            return null;
        }
    }
}