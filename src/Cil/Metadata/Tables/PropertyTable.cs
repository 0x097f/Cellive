namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class PropertyTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<PropertyRow> rows = new();

        public IReadOnlyList<PropertyRow> Rows => rows;
        public int Count => rows.Count;

        public PropertyTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Property);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Property);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new PropertyRow
                {
                    Flags = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                    TypeIndex = reader.ReadBlobIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Type = metadata.Blobs.Get(row.TypeIndex);

                rows.Add(row);
            }
        }

        public PropertyRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public PropertyRow? FindByName(string name)
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