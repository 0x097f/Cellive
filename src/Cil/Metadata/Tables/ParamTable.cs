namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ParamTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ParamRow> rows = new();

        public IReadOnlyList<ParamRow> Rows => rows;
        public int Count => rows.Count;

        public ParamTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Param);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Param);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ParamRow
                {
                    Flags = reader.ReadUInt16(),
                    Sequence = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                rows.Add(row);
            }
        }

        public ParamRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}