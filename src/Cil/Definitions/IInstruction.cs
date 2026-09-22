using Cellive;

namespace Cellive.Cil.Definitions
{
    public interface IInstruction
    {
        int Offset { get; }
        OpCode OpCode { get; }
        object? Operand { get; }
        int Size { get; }

        bool HasOperand { get; }

        int? BranchTargetOffset { get; }
        int[]? SwitchTargets { get; }

        IInstruction? Previous { get; }
        IInstruction? Next { get; }
        string? Label { get; }
        bool IsBranchTarget { get; }
    }
}