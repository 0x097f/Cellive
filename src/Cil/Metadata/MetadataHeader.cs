using System;

namespace Cellive.Cil.Metadata
{
    public sealed class MetadataHeader
    {
        public const uint ExpectedSignature = 0x424A5342; // BSJB

        public uint Signature { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public uint Reserved { get; set; }
        public uint VersionLength { get; set; }
        public string VersionString { get; set; } = string.Empty;
        public ushort Flags { get; set; }
        public ushort StreamCount { get; set; }

        public bool IsValid => Signature == ExpectedSignature;

        public static MetadataHeader Read(BinaryReader reader)
        {
            var header = new MetadataHeader
            {
                Signature = reader.ReadUInt32(),
                MajorVersion = reader.ReadUInt16(),
                MinorVersion = reader.ReadUInt16(),
                Reserved = reader.ReadUInt32(),
                VersionLength = reader.ReadUInt32(),
            };

            if (header.VersionLength > 0 && header.VersionLength < 256)
            {
                var bytes = reader.ReadBytes((int)header.VersionLength);
                header.VersionString = Encoding.UTF8.GetString(bytes).TrimEnd('\0');
            }

            Align(reader, 4);

            header.Flags = reader.ReadUInt16();
            header.StreamCount = reader.ReadUInt16();

            return header;
        }

        private static void Align(BinaryReader reader, int alignment)
        {
            var pos = reader.BaseStream.Position;
            var mod = pos % alignment;
            if (mod != 0)
                reader.BaseStream.Seek(alignment - mod, SeekOrigin.Current);
        }

        public override string ToString() =>
            $"Metadata v{MajorVersion}.{MinorVersion} \"{VersionString}\" ({StreamCount} streams)";
    }
}