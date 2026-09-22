namespace Cellive.Cil.Metadata.Tables
{
    public sealed class FileTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<FileRow> rows = new();

        public IReadOnlyList<FileRow> Rows => rows;
        public int Count => rows.Count;

        public FileTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.File);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.File);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new FileRow
                {
                    Flags = reader.ReadUInt32(),
                    NameIndex = reader.ReadStringIndex(),
                    HashValueIndex = reader.ReadBlobIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.HashValue = metadata.Blobs.Get(row.HashValueIndex);

                rows.Add(row);
            }
        }

        public FileRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public FileRow? FindByName(string name)
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