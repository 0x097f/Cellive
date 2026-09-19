namespace CilDotNet.Executable
{
    /// <summary>
    /// Reference : https://learn.microsoft.com/windows/win32/debug/pe-format
    /// </summary>
    [Flags]
    public enum Subsystem : ushort
    {
        Unknown = 0x0000,
        Native = 0x0001,
        WindowsGui = 0x0002,
        WindowsCui = 0x0003,
        Os2Cui = 0x0005,
        PosixCui = 0x0007,
        NativeWindows = 0x0008,
        WindowsCeGui = 0x0009,
        EfiApplication = 0x000A,
        EfiBootServiceDriver = 0x000B,
        EfiRuntimeDriver = 0x000C,
        EfiRom = 0x000D,
        Xbox = 0x000E,
        WindowsBootApplication = 0x0010,
    }
    public static class SubsystemExtensions
    {
        public static string GetFriendlyName(this Subsystem value)
        {
            return value switch
            {
                Subsystem.Unknown => "Unknown",
                Subsystem.Native => "Native",
                Subsystem.WindowsGui => "Windows GUI",
                Subsystem.WindowsCui => "Windows Console",
                Subsystem.Os2Cui => "OS/2 Console",
                Subsystem.PosixCui => "POSIX Console",
                Subsystem.NativeWindows => "Native Windows 9x",
                Subsystem.WindowsCeGui => "Windows CE GUI",
                Subsystem.EfiApplication => "EFI Application",
                Subsystem.EfiBootServiceDriver => "EFI Boot Service Driver",
                Subsystem.EfiRuntimeDriver => "EFI Runtime Driver",
                Subsystem.EfiRom => "EFI ROM",
                Subsystem.Xbox => "Xbox",
                Subsystem.WindowsBootApplication => "Windows Boot Application",
                _ => $"Unknown (0x{(ushort)value:X4})"
            };
        }

        public static Subsystem Parse(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return Subsystem.Unknown;

            return value.ToLowerInvariant() switch
            {
                "windows" or "windows_gui" or "gui" => Subsystem.WindowsGui,
                "console" or "windows_cui" or "cui" => Subsystem.WindowsCui,
                "native" => Subsystem.Native,
                "posix" => Subsystem.PosixCui,
                "os2" or "os2_cui" => Subsystem.Os2Cui,
                "efi_application" => Subsystem.EfiApplication,
                "efi_boot_service_driver" => Subsystem.EfiBootServiceDriver,
                "efi_runtime_driver" => Subsystem.EfiRuntimeDriver,
                "efi_rom" => Subsystem.EfiRom,
                "xbox" => Subsystem.Xbox,
                "boot_application" => Subsystem.WindowsBootApplication,
                _ => Subsystem.Unknown
            };
        }

        public static Subsystem ReadFromPE(byte[] peData)
        {
            if (peData.Length < 0x40)
                return Subsystem.Unknown;

            int e_lfanew = BitConverter.ToInt32(peData, 0x3C);

            if (e_lfanew + 4 > peData.Length)
                return Subsystem.Unknown;

            int magicOffset = e_lfanew + 0x18;
            if (magicOffset + 2 > peData.Length)
                return Subsystem.Unknown;

            ushort magic = BitConverter.ToUInt16(peData, magicOffset);

            int subsystemOffset;
            if (magic == 0x10B)
            {
                subsystemOffset = e_lfanew + 0x18 + 0x5C;
            }
            else if (magic == 0x20B)
            {
                subsystemOffset = e_lfanew + 0x18 + 0x64;
            }
            else
            {
                return Subsystem.Unknown;
            }

            if (subsystemOffset + 2 > peData.Length)
                return Subsystem.Unknown;

            ushort subsystemValue = BitConverter.ToUInt16(peData, subsystemOffset);
            return (Subsystem)subsystemValue;
        }
    }
}