namespace Cellive.Cil.Metadata.Tables
{
    public sealed class AssemblyRefTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<AssemblyRefRow> rows = new();

        public IReadOnlyList<AssemblyRefRow> Rows => rows;
        public int Count => rows.Count;

        public AssemblyRefTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.AssemblyRef);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.AssemblyRef);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new AssemblyRefRow
                {
                    MajorVersion = reader.ReadUInt16(),
                    MinorVersion = reader.ReadUInt16(),
                    BuildNumber = reader.ReadUInt16(),
                    RevisionNumber = reader.ReadUInt16(),
                    Flags = reader.ReadUInt32(),
                    PublicKeyOrTokenIndex = reader.ReadBlobIndex(),
                    NameIndex = reader.ReadStringIndex(),
                    CultureIndex = reader.ReadStringIndex(),
                    HashValueIndex = reader.ReadBlobIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Culture = metadata.Strings.Get(row.CultureIndex);
                row.PublicKeyOrToken = metadata.Blobs.Get(row.PublicKeyOrTokenIndex);
                row.HashValue = metadata.Blobs.Get(row.HashValueIndex);

                rows.Add(row);
            }
        }

        public AssemblyRefRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public AssemblyRefRow? FindByName(string name)
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