namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FieldTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<FieldRow> rows = new();

        public IReadOnlyList<FieldRow> Rows => rows;
        public int Count => rows.Count;

        public FieldTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Field);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Field);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new FieldRow
                {
                    Flags = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                    SignatureIndex = reader.ReadBlobIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Signature = metadata.Blobs.Get(row.SignatureIndex);
                rows.Add(row);
            }
        }

        public FieldRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public FieldRow? FindByName(string name)
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