namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MemberRefTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MemberRefRow> rows = new();

        public IReadOnlyList<MemberRefRow> Rows => rows;
        public int Count => rows.Count;

        public MemberRefTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.MemberRef);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.MemberRef);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new MemberRefRow
                {
                    Class = reader.ReadCodedIndex(CodedIndex.MemberRefParent).Value,
                    NameIndex = reader.ReadStringIndex(),
                    SignatureIndex = reader.ReadBlobIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Signature = metadata.Blobs.Get(row.SignatureIndex);
                row.ClassToken = new MetadataToken(row.Class);
                rows.Add(row);
            }
        }

        public MemberRefRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public MemberRefRow? FindByName(string name)
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