namespace Cellive.Cil.Metadata.Tables
{
    public sealed class CustomAttributeTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<CustomAttributeRow> rows = new();

        public IReadOnlyList<CustomAttributeRow> Rows => rows;
        public int Count => rows.Count;

        public CustomAttributeTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.CustomAttribute);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.CustomAttribute);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new CustomAttributeRow
                {
                    Parent = reader.ReadCodedIndex(CodedIndex.HasCustomAttribute).Value,
                    Type = reader.ReadCodedIndex(CodedIndex.CustomAttributeType).Value,
                    ValueIndex = reader.ReadBlobIndex(),
                };

                row.ParentToken = new MetadataToken(row.Parent);
                row.TypeToken = new MetadataToken(row.Type);
                row.Value = metadata.Blobs.Get(row.ValueIndex);

                rows.Add(row);
            }
        }

        public CustomAttributeRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<CustomAttributeRow> FindByParent(MetadataToken parent)
        {
            foreach (var row in rows)
            {
                if (row.ParentToken == parent)
                    yield return row;
            }
        }

        public IEnumerable<CustomAttributeRow> FindByType(MetadataToken type)
        {
            foreach (var row in rows)
            {
                if (row.TypeToken == type)
                    yield return row;
            }
        }
    }
}