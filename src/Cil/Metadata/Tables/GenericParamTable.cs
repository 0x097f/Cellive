namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class GenericParamTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<GenericParamRow> rows = new();

        public IReadOnlyList<GenericParamRow> Rows => rows;
        public int Count => rows.Count;

        public GenericParamTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.GenericParam);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.GenericParam);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new GenericParamRow
                {
                    Number = reader.ReadUInt16(),
                    Flags = reader.ReadUInt16(),
                    Owner = reader.ReadCodedIndex(CodedIndex.TypeOrMethodDef).Value,
                    NameIndex = reader.ReadStringIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.OwnerToken = new MetadataToken(row.Owner);

                rows.Add(row);
            }
        }

        public GenericParamRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<GenericParamRow> FindByOwner(MetadataToken owner)
        {
            foreach (var row in rows)
            {
                if (row.OwnerToken == owner)
                    yield return row;
            }
        }
    }
}