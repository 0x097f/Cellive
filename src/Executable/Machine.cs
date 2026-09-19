namespace CilDotNet.Executable
{
    [Flags]
    public enum Architectures : ushort
    {
        Unknown = 0x0000,
        I386 = 0x014C,
        MipsR3000 = 0x0162,
        MipsR4000 = 0x0166,
        MipsR10000 = 0x0168,
        MipsWceV2 = 0x0169,
        MipsBigEndian = 0x0288,
        MipsLittleEndian = 0x0366,
        RiscV32 = 0x5032,
        RiscV64 = 0x5064,
        RiscV128 = 0x5128,
        Alpha = 0x0184,
        Alpha64 = 0x0284,
        Arm = 0x01C0,
        ArmThumb2 = 0x01C4,
        Arm64 = 0xAA64,
        Amd64 = 0x8664,
        Ia64 = 0x0200,
        Sh3 = 0x01A2,
        Sh3Dsp = 0x01A3,
        Sh4 = 0x01A6,
        Sh5 = 0x01A8,
        ArmThumb = 0x01C2,
        Am33 = 0x01D3,
        PowerPc = 0x01F0,
        PowerPcBigEndian = 0x01F1,
        PowerPc64 = 0x01F5,
        TriCore = 0x0520,
        M32R = 0x9041,
        Xbox360 = 0x01F7,
        Ebc = 0x0EBC,
        Msp430 = 0x0203,
    }

    public static class ArchitecturesExtensions
    {
        public static string FriendlyName(this Architectures value)
        {
            return value switch
            {
                Architectures.Unknown => "Unknown",
                Architectures.I386 => "x86 (Intel 386+)",
                Architectures.MipsR3000 => "MIPS R3000 (BE)",
                Architectures.MipsR4000 => "MIPS R4000 (LE)",
                Architectures.MipsR10000 => "MIPS R10000 (LE)",
                Architectures.MipsWceV2 => "MIPS WCE v2",
                Architectures.MipsBigEndian => "MIPS (BE)",
                Architectures.MipsLittleEndian => "MIPS (LE)",
                Architectures.RiscV32 => "RISC-V 32-bit",
                Architectures.RiscV64 => "RISC-V 64-bit",
                Architectures.RiscV128 => "RISC-V 128-bit",
                Architectures.Alpha => "Alpha AXP",
                Architectures.Alpha64 => "Alpha 64-bit",
                Architectures.Arm => "ARM 32-bit",
                Architectures.ArmThumb2 => "ARM Thumb-2",
                Architectures.Arm64 => "ARM 64-bit (ARMv8+)",
                Architectures.Amd64 => "AMD64 (x86-64)",
                Architectures.Ia64 => "Intel IA-64 (Itanium)",
                Architectures.Sh3 => "Hitachi SH3",
                Architectures.Sh3Dsp => "Hitachi SH3 DSP",
                Architectures.Sh4 => "Hitachi SH4",
                Architectures.Sh5 => "Hitachi SH5",
                Architectures.ArmThumb => "ARM Thumb",
                Architectures.Am33 => "AM33",
                Architectures.PowerPc => "PowerPC (LE)",
                Architectures.PowerPcBigEndian => "PowerPC (BE)",
                Architectures.PowerPc64 => "PowerPC 64-bit (LE)",
                Architectures.TriCore => "TriCore",
                Architectures.M32R => "M32R",
                Architectures.Xbox360 => "Xbox 360 (PowerPC)",
                Architectures.Ebc => "EFI Byte Code",
                Architectures.Msp430 => "MSP430",
                _ => $"Unknown (0x{(ushort)value:X4})"
            };
        }

        public static int PointerSize(this Architectures value)
        {
            return value switch
            {
                Architectures.I386 => 4,
                Architectures.Arm => 4,
                Architectures.ArmThumb => 4,
                Architectures.ArmThumb2 => 4,
                Architectures.MipsR3000 => 4,
                Architectures.MipsR4000 => 4,
                Architectures.MipsR10000 => 4,
                Architectures.MipsWceV2 => 4,
                Architectures.MipsBigEndian => 4,
                Architectures.MipsLittleEndian => 4,
                Architectures.PowerPc => 4,
                Architectures.PowerPcBigEndian => 4,
                Architectures.Sh3 => 4,
                Architectures.Sh3Dsp => 4,
                Architectures.Sh4 => 4,
                Architectures.Sh5 => 4,
                Architectures.Xbox360 => 4,
                Architectures.Amd64 => 8,
                Architectures.Arm64 => 8,
                Architectures.Alpha64 => 8,
                Architectures.Ia64 => 8,
                Architectures.PowerPc64 => 8,
                Architectures.RiscV64 => 8,
                Architectures.RiscV128 => 16,
                _ => 0
            };
        }

        public static Architectures ReadFromPE(byte[] peData)
        {
            if (peData.Length < 0x40)
                return Architectures.Unknown;

            int e_lfanew = BitConverter.ToInt32(peData, 0x3C);

            if (e_lfanew + 4 > peData.Length)
                return Architectures.Unknown;

            int machineOffset = e_lfanew + 0x04;

            if (machineOffset + 2 > peData.Length)
                return Architectures.Unknown;

            ushort machineValue = BitConverter.ToUInt16(peData, machineOffset);
            return (Architectures)machineValue;
        }

        public static string ToLinkerMachineString(this Architectures value)
        {
            return value switch
            {
                Architectures.I386 => "X86",
                Architectures.Amd64 => "X64",
                Architectures.Arm => "ARM",
                Architectures.ArmThumb => "ARM",
                Architectures.ArmThumb2 => "ARM",
                Architectures.Arm64 => "ARM64",
                Architectures.Ia64 => "IA64",
                Architectures.MipsR3000 => "MIPS",
                Architectures.MipsR4000 => "MIPS",
                Architectures.MipsR10000 => "MIPS",
                Architectures.MipsWceV2 => "MIPS",
                Architectures.MipsBigEndian => "MIPS",
                Architectures.MipsLittleEndian => "MIPS",
                Architectures.PowerPc => "PPC",
                Architectures.PowerPcBigEndian => "PPC",
                Architectures.PowerPc64 => "PPC64",
                Architectures.RiscV32 => "RISCV32",
                Architectures.RiscV64 => "RISCV64",
                Architectures.RiscV128 => "RISCV128",
                _ => value.ToString()
            };
        }
    }
}