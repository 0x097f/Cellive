namespace CilDotNet.Cil.Definitions
{
    public interface IMethodBody
    {
        int MaxStackSize { get; }
        int CodeSize { get; }
        bool InitLocals { get; }
        byte[] RawBytes { get; }

        IReadOnlyList<IInstruction> Instructions { get; }
        IReadOnlyList<ILocalVariable> Variables { get; }
        IReadOnlyList<IExceptionHandler> ExceptionHandlers { get; }

        bool HasInstructions { get; }
        bool HasVariables { get; }
        bool HasExceptionHandlers { get; }
    }
}