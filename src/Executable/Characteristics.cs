namespace CilDotNet.Executable
{
    /// <summary>
    /// Characteristics flags.
    /// Reference: https://docs.microsoft.com/en-us/windows/win32/debug/pe-format
    /// </summary>
    [Flags]
    public enum Characteristics : ushort
    {
        /// <summary>
        /// The file does not contain base relocations and must be loaded at its preferred base address.
        /// </summary>
        RelocsStripped = 0x0001,

        /// <summary>
        /// The file is an executable image (there are no unresolved external references).
        /// </summary>
        ExecutableImage = 0x0002,

        /// <summary>
        /// COFF line numbers have been stripped from the file.
        /// </summary>
        LineNumsStripped = 0x0004,

        /// <summary>
        /// COFF symbol table entries have been stripped from the file.
        /// </summary>
        LocalSymsStripped = 0x0008,

        /// <summary>
        /// The working set is aggressively trimmed. (Obsolete)
        /// </summary>
        AggresiveWsTrim = 0x0010,

        /// <summary>
        /// The application can handle addresses larger than 2 GB.
        /// </summary>
        LargeAddressAware = 0x0020,

        /// <summary>
        /// Reserved; must not be used.
        /// </summary>
        Reserved = 0x0040,

        /// <summary>
        /// The byte order is little endian. (Obsolete)
        /// </summary>
        BytesReversedLo = 0x0080,

        /// <summary>
        /// The machine supports 32-bit word length.
        /// </summary>
        Bit32Machine = 0x0100,

        /// <summary>
        /// Debugging information has been stripped and stored separately in a .dbg file.
        /// </summary>
        DebugStripped = 0x0200,

        /// <summary>
        /// If the image is on removable media, copy it to and run it from the swap file.
        /// </summary>
        RemovableRunFromSwap = 0x0400,

        /// <summary>
        /// If the image is on the network, copy it to and run it from the swap file.
        /// </summary>
        NetRunFromSwap = 0x0800,

        /// <summary>
        /// The image is a system file (e.g., a driver or a kernel lib) and cannot be run directly.
        /// </summary>
        SystemFile = 0x1000,

        /// <summary>
        /// The image is a dynamic-link library (DLL) file.
        /// </summary>
        Dll = 0x2000,

        /// <summary>
        /// The file should be run only on a uniprocessor system.
        /// </summary>
        UpSystemOnly = 0x4000,

        /// <summary>
        /// The byte order is big-endian.
        /// </summary>
        BytesReversedHi = 0x8000,
    }

    /// <summary>
    /// Extension methods for the Characteristics enum.
    /// </summary>
    public static class CharacteristicsExtensions
    {
        /// <summary>
        /// Determines whether the specified flag is set.
        /// </summary>
        public static bool Has(this Characteristics value, Characteristics flag)
        {
            return (value & flag) == flag;
        }

        /// <summary>
        /// Gets a human-readable description of the flags.
        /// </summary>
        public static string GetDescription(this Characteristics value)
        {
            if (value == 0)
                return "No flags";

            var descriptions = new List<string>();

            if (value.Has(Characteristics.RelocsStripped))
                descriptions.Add("Relocations stripped");
            if (value.Has(Characteristics.ExecutableImage))
                descriptions.Add("Executable");
            if (value.Has(Characteristics.LineNumsStripped))
                descriptions.Add("Line numbers stripped");
            if (value.Has(Characteristics.LocalSymsStripped))
                descriptions.Add("Symbols stripped");
            if (value.Has(Characteristics.AggresiveWsTrim))
                descriptions.Add("Aggressive WS trim [obsolete]");
            if (value.Has(Characteristics.LargeAddressAware))
                descriptions.Add("Large address aware");
            if (value.Has(Characteristics.Reserved))
                descriptions.Add("[Reserved]");
            if (value.Has(Characteristics.BytesReversedLo))
                descriptions.Add("Little-endian (obsolete)");
            if (value.Has(Characteristics.Bit32Machine))
                descriptions.Add("32-bit");
            if (value.Has(Characteristics.DebugStripped))
                descriptions.Add("Debug info stripped");
            if (value.Has(Characteristics.RemovableRunFromSwap))
                descriptions.Add("Removable swap run");
            if (value.Has(Characteristics.NetRunFromSwap))
                descriptions.Add("Network swap run");
            if (value.Has(Characteristics.SystemFile))
                descriptions.Add("System file");
            if (value.Has(Characteristics.Dll))
                descriptions.Add("DLL");
            if (value.Has(Characteristics.UpSystemOnly))
                descriptions.Add("Uniprocessor only");
            if (value.Has(Characteristics.BytesReversedHi))
                descriptions.Add("Big-endian");

            return descriptions.Count > 0 ? string.Join(", ", descriptions) : "Unknown";
        }

        /// <summary>
        /// Determines whether the image is executable.
        /// </summary>
        public static bool Executable(this Characteristics value)
        {
            return value.Has(Characteristics.ExecutableImage);
        }

        /// <summary>
        /// Determines whether the image is a DLL.
        /// </summary>
        public static bool Dll(this Characteristics value)
        {
            return value.Has(Characteristics.Dll);
        }

        /// <summary>
        /// Determines whether the image is a 32-bit executable.
        /// </summary>
        public static bool Bit32(this Characteristics value)
        {
            return value.Has(Characteristics.Bit32Machine);
        }

        /// <summary>
        /// Determines whether the image is a valid PE file (executable or DLL).
        /// </summary>
        public static bool ValidPe(this Characteristics value)
        {
            return value.Has(Characteristics.ExecutableImage) ||
                   value.Has(Characteristics.Dll);
        }

        /// <summary>
        /// Converts the flags to a hexadecimal string representation.
        /// </summary>
        public static string ToHexString(this Characteristics value)
        {
            return $"0x{(ushort)value:X4}";
        }
    }
}