//Use Iced.Intel
namespace Cellive.Disassembler
{
    public sealed class IcedDisassembler : IDisassembler
    {
        private readonly Formatter fmt;

        public IcedDisassembler()
        {
            //Standard output format
            fmt = new NasmFormatter();
        }

        public IReadOnlyList<DisassembledInstruction> Disassemble(byte[] code, ulong address, int bitness)
        {
            return Disassemble(code, address, bitness, int.MaxValue);
        }

        public IReadOnlyList<DisassembledInstruction> Disassemble(byte[] code, ulong address, int bitness, int maxInstructions)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            var result = new List<DisassembledInstruction>();
            var reader = new ByteArrayCodeReader(code);
            var decoder = Iced.Intel.Decoder.Create(bitness, reader, address);

            var output = new StringOutput();
            ulong endAddress = address + (ulong)code.Length;

            while (decoder.IP < endAddress && result.Count < maxInstructions)
            {
                var instruction = decoder.Decode();
                if (instruction.IsInvalid)
                    break;

                fmt.Format(instruction, output);

                var disassembled = new DisassembledInstruction
                {
                    Address = instruction.IP,
                    Length = instruction.Length,
                    Text = output.ToStringAndReset(),
                    Mnemonic = instruction.Mnemonic.ToString(),
                    Bytes = GetInstructionBytes(code, instruction.IP - address, instruction.Length),
                };

                var flow = instruction.FlowControl;
                if (flow == Iced.Intel.FlowControl.UnconditionalBranch ||
                    flow == Iced.Intel.FlowControl.ConditionalBranch ||
                    flow == Iced.Intel.FlowControl.Call ||
                    flow == Iced.Intel.FlowControl.IndirectBranch ||
                    flow == Iced.Intel.FlowControl.IndirectCall)
                {
                    disassembled.IsBranch = true;

                    if (instruction.IsIPRelativeMemoryOperand)
                    {
                        disassembled.BranchTarget = instruction.IPRelativeMemoryAddress;
                    }
                    else if (flow == Iced.Intel.FlowControl.UnconditionalBranch || flow == Iced.Intel.FlowControl.ConditionalBranch)
                    {
                        var target = instruction.NearBranchTarget;
                        if (target != 0)
                            disassembled.BranchTarget = target;
                    }
                }

                result.Add(disassembled);
            }

            return result;
        }

        private static byte[] GetInstructionBytes(byte[] code, ulong offset, int length)
        {
            var bytes = new byte[length];
            int start = (int)offset;
            for (int i = 0; i < length; i++)
            {
                if (start + i < code.Length)
                    bytes[i] = code[start + i];
            }
            return bytes;
        }
    }
}