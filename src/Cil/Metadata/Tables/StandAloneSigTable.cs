namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class StandAloneSigTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<StandAloneSigRow> rows = new();

        public IReadOnlyList<StandAloneSigRow> Rows => rows;
        public int Count => rows.Count;

        public StandAloneSigTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.StandAloneSig);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.StandAloneSig);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new StandAloneSigRow
                {
                    SignatureIndex = reader.ReadBlobIndex(),
                };

                row.Signature = metadata.Blobs.Get(row.SignatureIndex);
                rows.Add(row);
            }
        }

        public StandAloneSigRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}