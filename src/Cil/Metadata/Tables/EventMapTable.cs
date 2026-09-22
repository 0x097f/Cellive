namespace Cellive.Cil.Metadata.Tables
{
    public sealed class EventMapTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<EventMapRow> rows = new();

        public IReadOnlyList<EventMapRow> Rows => rows;
        public int Count => rows.Count;

        public EventMapTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.EventMap);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.EventMap);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new EventMapRow
                {
                    Parent = reader.ReadTableIndex(TableType.TypeDef),
                    EventList = reader.ReadTableIndex(TableType.Event),
                };

                row.ParentToken = MetadataToken.FromToken(TableType.TypeDef, row.Parent);
                row.EventListToken = MetadataToken.FromToken(TableType.Event, row.EventList);

                rows.Add(row);
            }
        }

        public EventMapRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public EventMapRow? FindByParent(MetadataToken parent)
        {
            foreach (var row in rows)
            {
                if (row.ParentToken == parent)
                    return row;
            }
            return null;
        }
    }
}