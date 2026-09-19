namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ConstantTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ConstantRow> rows = new();

        public IReadOnlyList<ConstantRow> Rows => rows;
        public int Count => rows.Count;

        public ConstantTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Constant);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Constant);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ConstantRow
                {
                    Type = reader.ReadByte(),
                    Padding = reader.ReadByte(),
                    Parent = reader.ReadCodedIndex(CodedIndex.HasConstant).Value,
                    ValueIndex = reader.ReadBlobIndex(),
                };

                row.ParentToken = new MetadataToken(row.Parent);
                row.Value = metadata.Blobs.Get(row.ValueIndex);

                rows.Add(row);
            }
        }

        public ConstantRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ConstantRow? FindByParent(MetadataToken parent)
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