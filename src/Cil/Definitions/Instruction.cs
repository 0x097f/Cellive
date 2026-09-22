using Cellive;

namespace Cellive.Cil.Definitions
{
    public sealed class Instruction : IInstruction
    {
        public int Offset { get; set; }
        public OpCode OpCode { get; set; } = OpCodeTable.Nop;
        public object? Operand { get; set; }
        public int Size { get; set; }

        public bool HasOperand => Operand != null;

        public int? BranchTargetOffset
        {
            get
            {
                if (Operand is int target)
                    return target;
                return null;
            }
        }

        public int[]? SwitchTargets
        {
            get
            {
                if (Operand is int[] targets)
                    return targets;
                return null;
            }
        }

        public IInstruction? Previous { get; set; }
        public IInstruction? Next { get; set; }
        public string? Label { get; set; }
        public bool IsBranchTarget { get; set; }

        public string GetOperandDisplay()
        {
            if (Operand == null)
                return string.Empty;

            return Operand switch
            {
                int i => $"IL_{i:X4}",
                long l => l.ToString(),
                float f => f.ToString(System.Globalization.CultureInfo.InvariantCulture),
                double d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
                string s => $"\"{s}\"",
                int[] targets => $"switch ({targets.Length} targets)",
                _ => Operand.ToString() ?? string.Empty
            };
        }

        public override string ToString()
        {
            var operand = GetOperandDisplay();
            return $"IL_{Offset:X4}: {OpCode?.Name ?? "???"} {operand}".TrimEnd();
        }
    }
}
