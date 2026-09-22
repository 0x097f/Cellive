namespace Cellive.Cil.Metadata.Tables
{
    public sealed class MethodSemanticsTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MethodSemanticsRow> rows = new();

        public IReadOnlyList<MethodSemanticsRow> Rows => rows;
        public int Count => rows.Count;

        public MethodSemanticsTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.MethodSemantics);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.MethodSemantics);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new MethodSemanticsRow
                {
                    Semantics = reader.ReadUInt16(),
                    Method = reader.ReadTableIndex(TableType.Method),
                    Association = reader.ReadCodedIndex(CodedIndex.HasSemantics).Value,
                };

                row.MethodToken = MetadataToken.FromToken(TableType.Method, row.Method);
                row.AssociationToken = new MetadataToken(row.Association);

                rows.Add(row);
            }
        }

        public MethodSemanticsRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<MethodSemanticsRow> FindByAssociation(MetadataToken association)
        {
            foreach (var row in rows)
            {
                if (row.AssociationToken == association)
                    yield return row;
            }
        }

        public MethodSemanticsRow? FindGetter(MetadataToken association)
        {
            foreach (var row in rows)
            {
                if (row.AssociationToken == association && row.IsGetter)
                    return row;
            }
            return null;
        }

        public MethodSemanticsRow? FindSetter(MetadataToken association)
        {
            foreach (var row in rows)
            {
                if (row.AssociationToken == association && row.IsSetter)
                    return row;
            }
            return null;
        }

        public MethodSemanticsRow? FindAddOn(MetadataToken association)
        {
            foreach (var row in rows)
            {
                if (row.AssociationToken == association && row.IsAddOn)
                    return row;
            }
            return null;
        }

        public MethodSemanticsRow? FindRemoveOn(MetadataToken association)
        {
            foreach (var row in rows)
            {
                if (row.AssociationToken == association && row.IsRemoveOn)
                    return row;
            }
            return null;
        }
    }
}