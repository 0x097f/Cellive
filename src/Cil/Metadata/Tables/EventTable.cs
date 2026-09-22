namespace Cellive.Cil.Metadata.Tables
{
    public sealed class EventTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<EventRow> rows = new();

        public IReadOnlyList<EventRow> Rows => rows;
        public int Count => rows.Count;

        public EventTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Event);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Event);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new EventRow
                {
                    Flags = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                    EventType = reader.ReadCodedIndex(CodedIndex.TypeDefOrRef).Value,
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.EventTypeToken = new MetadataToken(row.EventType);

                rows.Add(row);
            }
        }

        public EventRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public EventRow? FindByName(string name)
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