namespace CilDotNet.Disassembler
{
    public interface IDisassembler
    {
        IReadOnlyList<DisassembledInstruction> Disassemble(byte[] code, ulong address, int bitness);
        IReadOnlyList<DisassembledInstruction> Disassemble(byte[] code, ulong address, int bitness, int maxInstructions);
    }
}