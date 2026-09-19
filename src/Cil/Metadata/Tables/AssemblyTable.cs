namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class AssemblyTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<AssemblyRow> rows = new();

        public IReadOnlyList<AssemblyRow> Rows => rows;
        public int Count => rows.Count;

        public AssemblyTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Assembly);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Assembly);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new AssemblyRow
                {
                    HashAlgId = reader.ReadUInt32(),
                    MajorVersion = reader.ReadUInt16(),
                    MinorVersion = reader.ReadUInt16(),
                    BuildNumber = reader.ReadUInt16(),
                    RevisionNumber = reader.ReadUInt16(),
                    Flags = reader.ReadUInt32(),
                    PublicKeyIndex = reader.ReadBlobIndex(),
                    NameIndex = reader.ReadStringIndex(),
                    CultureIndex = reader.ReadStringIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Culture = metadata.Strings.Get(row.CultureIndex);
                row.PublicKey = metadata.Blobs.Get(row.PublicKeyIndex);

                rows.Add(row);
            }
        }

        public AssemblyRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public AssemblyRow? First => rows.Count > 0 ? rows[0] : null;
    }
}