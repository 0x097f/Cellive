namespace Cellive.Cil.Metadata.Tables
{
    public sealed class MethodSpecTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MethodSpecRow> rows = new();

        public IReadOnlyList<MethodSpecRow> Rows => rows;
        public int Count => rows.Count;

        public MethodSpecTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.MethodSpec);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.MethodSpec);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new MethodSpecRow
                {
                    Method = reader.ReadCodedIndex(CodedIndex.MethodDefOrRef).Value,
                    InstantiationIndex = reader.ReadBlobIndex(),
                };

                row.MethodToken = new MetadataToken(row.Method);
                row.Instantiation = metadata.Blobs.Get(row.InstantiationIndex);

                rows.Add(row);
            }
        }

        public MethodSpecRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}