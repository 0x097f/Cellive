namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ManifestResourceTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ManifestResourceRow> rows = new();

        public IReadOnlyList<ManifestResourceRow> Rows => rows;
        public int Count => rows.Count;

        public ManifestResourceTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ManifestResource);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ManifestResource);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ManifestResourceRow
                {
                    Offset = reader.ReadUInt32(),
                    Flags = reader.ReadUInt32(),
                    NameIndex = reader.ReadStringIndex(),
                    Implementation = reader.ReadCodedIndex(CodedIndex.Implementation).Value,
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.ImplementationToken = new MetadataToken(row.Implementation);

                rows.Add(row);
            }
        }

        public ManifestResourceRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ManifestResourceRow? FindByName(string name)
        {
            foreach (var row in rows)
            {
                if (row.Name == name)
                    return row;
            }
            return null;
        }

        public IEnumerable<ManifestResourceRow> GetEmbedded()
        {
            foreach (var row in rows)
            {
                if (row.IsEmbedded)
                    yield return row;
            }
        }
    }
}