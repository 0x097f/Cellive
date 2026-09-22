namespace Cellive.Executable
{
    public readonly struct Rva : IEquatable<Rva>
    {
        private static List<SectionInfo>? sc;

        public uint Value { get; }

        public Rva(uint value)
        {
            Value = value;
        }

        public static void SetSections(List<SectionInfo> sections)
        {
            sc = sections;
        }

        public BinaryOffset ToFileOffset()
        {
            if (sc == null)
            {
                //You need to call Rva.SetSections first
                throw new InvalidOperationException("Sections not set.");
            }

            foreach (var section in sc)
            {
                var size = Math.Max(section.VirtualSize, section.SizeOfRawData);
                if (Value >= section.VirtualAddress && Value < section.VirtualAddress + size)
                {
                    var offset = Value - section.VirtualAddress + section.PointerToRawData;
                    return new BinaryOffset(offset);
                }
            }
            return new BinaryOffset(0);
        }

        public bool TryToFileOffset(out BinaryOffset offset)
        {
            if (sc == null)
            {
                offset = default;
                return false;
            }

            foreach (var section in sc)
            {
                if (Value >= section.VirtualAddress &&
                    Value < section.VirtualAddress + section.VirtualSize)
                {
                    var fileOffset = Value - section.VirtualAddress + section.PointerToRawData;
                    offset = new BinaryOffset(fileOffset);
                    return true;
                }
            }

            offset = default;
            return false;
        }

        public SectionInfo? Section
        {
            get
            {
                if (sc == null) return null;
                foreach (var section in sc)
                {
                    var size = Math.Max(section.VirtualSize, section.SizeOfRawData);
                    if (Value >= section.VirtualAddress && Value < section.VirtualAddress + size)
                    {
                        return section;
                    }
                }
                return null;
            }
        }

        public bool InSection(string sectionName)
        {
            var section = Section;
            return section != null && section.Name.TrimEnd('\0') == sectionName;
        }

        public static implicit operator uint(Rva rva) => rva.Value;
        public static implicit operator Rva(uint value) => new Rva(value);
        public static implicit operator Rva(int value) => new Rva((uint)value);

        public static Rva operator +(Rva rva, int offset) => new Rva((uint)(rva.Value + offset));
        public static Rva operator -(Rva rva, int offset) => new Rva((uint)(rva.Value - offset));
        public static Rva operator +(Rva rva, uint offset) => new Rva(rva.Value + offset);
        public static Rva operator -(Rva rva, uint offset) => new Rva(rva.Value - offset);

        public static bool operator ==(Rva left, Rva right) => left.Value == right.Value;
        public static bool operator !=(Rva left, Rva right) => left.Value != right.Value;

        public bool Equals(Rva other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is Rva other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"0x{Value:X8}";

        public bool IsValid => Value != 0;
        public bool IsNull => Value == 0;
    }
}
