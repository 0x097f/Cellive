namespace CilDotNet.Disassembler
{
    public sealed class DisassembledInstruction
    {
        public ulong Address { get; set; }
        public int Length { get; set; }
        public string Text { get; set; }
        public string Mnemonic { get; set; }
        public byte[] Bytes { get; set; }
        public bool IsBranch { get; set; }
        public ulong? BranchTarget { get; set; }
    }
}