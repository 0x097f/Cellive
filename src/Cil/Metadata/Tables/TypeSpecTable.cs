namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class TypeSpecTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<TypeSpecRow> rows = new();

        public IReadOnlyList<TypeSpecRow> Rows => rows;
        public int Count => rows.Count;

        public TypeSpecTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.TypeSpec);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.TypeSpec);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new TypeSpecRow
                {
                    SignatureIndex = reader.ReadBlobIndex(),
                };

                row.Signature = metadata.Blobs.Get(row.SignatureIndex);
                rows.Add(row);
            }
        }

        public TypeSpecRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}