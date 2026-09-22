namespace Cellive.Cil.Definitions
{
    public sealed class MethodBody : IMethodBody
    {
        public int MaxStackSize { get; set; } = 8;
        public int CodeSize { get; set; }
        public bool InitLocals { get; set; }
        public byte[] RawBytes { get; set; } = System.Array.Empty<byte>();

        public List<IInstruction> InstructionsList { get; } = new();
        public List<ILocalVariable> VariablesList { get; } = new();
        public List<IExceptionHandler> ExceptionHandlersList { get; } = new();

        public IReadOnlyList<IInstruction> Instructions => InstructionsList;
        public IReadOnlyList<ILocalVariable> Variables => VariablesList;
        public IReadOnlyList<IExceptionHandler> ExceptionHandlers => ExceptionHandlersList;

        public bool HasInstructions => InstructionsList.Count > 0;
        public bool HasVariables => VariablesList.Count > 0;
        public bool HasExceptionHandlers => ExceptionHandlersList.Count > 0;

        public override string ToString() =>
            $"MaxStack={MaxStackSize} CodeSize={CodeSize} Instructions={InstructionsList.Count} Variables={VariablesList.Count}";
    }
}