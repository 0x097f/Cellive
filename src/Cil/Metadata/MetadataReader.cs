namespace CilDotNet.Cil.Metadata
{
    public sealed class MetadataReader : IDisposable
    {
        private readonly PeImage pe;
        private readonly BinaryReader reader;
        private readonly ulong metadataOffset;
        private readonly Dictionary<TableType, TableInfo> tables = new();
        private readonly StringHeap stringHeap;
        private readonly BlobHeap blobHeap;
        private readonly GuidHeap guidHeap;
        private readonly UserStringHeap userStringHeap;
        private bool disposed;

        public MetadataHeader Header { get; }
        public StreamHeaders StreamHeaders { get; }
        public HeapSizes HeapSizes { get; }
        public IReadOnlyDictionary<TableType, TableInfo> Tables => tables;
        public string VersionString { get; }

        public StringHeap Strings => stringHeap;
        public BlobHeap Blobs => blobHeap;
        public GuidHeap Guids => guidHeap;
        public UserStringHeap UserStrings => userStringHeap;

        public MetadataReader(PeImage pe)
        {
            this.pe = pe ?? throw new ArgumentNullException(nameof(pe));

            var cliHeader = pe.CliHeader
                ?? throw new InvalidDataException("Not a managed assembly");

            var metaDir = cliHeader.MetaData;
            if (metaDir.IsEmpty)
                throw new InvalidDataException("No metadata directory");

            var metaBytes = pe.ReadRva(metaDir.VirtualAddress, (int)metaDir.Size);
            if (metaBytes.Length == 0)
                throw new InvalidDataException("Metadata directory is empty");

            var stream = new MemoryStream(metaBytes);
            reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            Header = MetadataHeader.Read(reader);
            VersionString = Header.VersionString;

            StreamHeaders = StreamHeaders.Read(reader, Header);

            metadataOffset = 0;

            stringHeap = new StringHeap();
            blobHeap = new BlobHeap();
            guidHeap = new GuidHeap();
            userStringHeap = new UserStringHeap();

            ReadStreams();

            ReadTables();
        }

        private void ReadStreams()
        {
            foreach (var stream in StreamHeaders.Streams)
            {
                switch (stream.Name)
                {
                    case "#Strings":
                        stringHeap.Read(reader, stream.Offset, stream.Size);
                        break;
                    case "#Blob":
                        blobHeap.Read(reader, stream.Offset, stream.Size);
                        break;
                    case "#GUID":
                        guidHeap.Read(reader, stream.Offset, stream.Size);
                        break;
                    case "#US":
                        userStringHeap.Read(reader, stream.Offset, stream.Size);
                        break;
                }
            }
        }

        private void ReadTables()
        {
            var tableStream = StreamHeaders.GetStream("#~") ?? StreamHeaders.GetStream("#-");
            if (tableStream == null)
                throw new InvalidDataException("No table stream (#~ or #-)");

            reader.BaseStream.Seek(tableStream.Offset, SeekOrigin.Begin);

            reader.ReadUInt32();                       // Reserved
            reader.ReadByte();                         // MajorVersion
            reader.ReadByte();                         // MinorVersion
            var heapSizes = (HeapSizes)reader.ReadByte();
            reader.ReadByte();                         // Reserved
            var valid = reader.ReadUInt64();
            var sorted = reader.ReadUInt64();

            for (int i = 0; i < 64; i++)
            {
                if ((valid & (1UL << i)) == 0)
                    continue;

                var rowCount = reader.ReadUInt32();
                var tableType = (TableType)i;

                tables[tableType] = new TableInfo
                {
                    Type = tableType,
                    RowCount = (int)rowCount,
                    Offset = reader.BaseStream.Position,
                    RowSize = 0,
                };
            }
        }

        public TableInfo GetTableInfo(TableType type)
        {
            return tables.TryGetValue(type, out var info) ? info : default;
        }

        public bool HasTable(TableType type) => tables.ContainsKey(type);

        public int GetRowCount(TableType type)
        {
            return tables.TryGetValue(type, out var info) ? info.RowCount : 0;
        }

        public bool IsStringHeapLarge => (HeapSizes & HeapSizes.StringHeapLarge) != 0;
        public bool IsGuidHeapLarge => (HeapSizes & HeapSizes.GuidHeapLarge) != 0;
        public bool IsBlobHeapLarge => (HeapSizes & HeapSizes.BlobHeapLarge) != 0;

        public void Dispose()
        {
            if (!disposed)
            {
                reader?.Dispose();
                disposed = true;
            }
        }
    }
}