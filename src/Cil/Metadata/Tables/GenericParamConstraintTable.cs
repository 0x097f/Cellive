namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class GenericParamConstraintTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<GenericParamConstraintRow> rows = new();

        public IReadOnlyList<GenericParamConstraintRow> Rows => rows;
        public int Count => rows.Count;

        public GenericParamConstraintTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.GenericParamConstraint);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.GenericParamConstraint);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new GenericParamConstraintRow
                {
                    Owner = reader.ReadTableIndex(TableType.GenericParam),
                    Constraint = reader.ReadCodedIndex(CodedIndex.TypeDefOrRef).Value,
                };

                row.OwnerToken = MetadataToken.FromToken(TableType.GenericParam, row.Owner);
                row.ConstraintToken = new MetadataToken(row.Constraint);

                rows.Add(row);
            }
        }

        public GenericParamConstraintRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<GenericParamConstraintRow> FindByOwner(MetadataToken owner)
        {
            foreach (var row in rows)
            {
                if (row.OwnerToken == owner)
                    yield return row;
            }
        }
    }
}