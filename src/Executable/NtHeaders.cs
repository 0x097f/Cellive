// NtHeaders.cs
using System;
using System.IO;

namespace Cellive.Executable
{
    public sealed class NtHeaders
    {
        public const uint PeSignature = 0x00004550; //PE\0\0

        public uint Signature { get; set; }
        public FileHeader FileHeader { get; set; } = new();
        public OptionalHeader OptionalHeader { get; set; } = new();

        public bool IsValid => Signature == PeSignature;

        public bool Is64Bit => OptionalHeader.Is64Bit;

        public static NtHeaders Read(BinaryReader reader, long peOffset)
        {
            reader.BaseStream.Seek(peOffset, SeekOrigin.Begin);

            var nt = new NtHeaders
            {
                Signature = reader.ReadUInt32(),
            };

            nt.FileHeader = FileHeader.Read(reader);
            nt.OptionalHeader = OptionalHeader.Read(reader, nt.FileHeader.SizeOfOptionalHeader);

            return nt;
        }

        public override string ToString() =>
            $"NT Headers (Sig=0x{Signature:X8}, Machine={FileHeader.Machine}, {(Is64Bit ? "PE32+" : "PE32")})";
    }

    public sealed class FileHeader
    {
        public ushort Machine { get; set; }
        public ushort NumberOfSections { get; set; }
        public uint TimeDateStamp { get; set; }
        public uint PointerToSymbolTable { get; set; }
        public uint NumberOfSymbols { get; set; }
        public ushort SizeOfOptionalHeader { get; set; }
        public ushort Characteristics { get; set; }

        public bool IsDll => (Characteristics & 0x2000) != 0;
        public bool IsExecutable => (Characteristics & 0x0002) != 0;

        public Architectures Architecture => (Architectures)Machine;

        public static FileHeader Read(BinaryReader reader)
        {
            return new FileHeader
            {
                Machine = reader.ReadUInt16(),
                NumberOfSections = reader.ReadUInt16(),
                TimeDateStamp = reader.ReadUInt32(),
                PointerToSymbolTable = reader.ReadUInt32(),
                NumberOfSymbols = reader.ReadUInt32(),
                SizeOfOptionalHeader = reader.ReadUInt16(),
                Characteristics = reader.ReadUInt16(),
            };
        }

        public override string ToString() =>
            $"FileHeader Machine={Architecture.FriendlyName()} Sections={NumberOfSections}";
    }
}