namespace CilDotNet.Cil.Definitions
{
    public interface IExceptionHandler
    {
        ExceptionHandlerType HandlerType { get; }

        int TryStart { get; }
        int TryEnd { get; }
        int HandlerStart { get; }
        int HandlerEnd { get; }
        int FilterStart { get; }

        ITypeDef? CatchType { get; }
        bool HasFilter { get; }
    }

    public enum ExceptionHandlerType
    {
        Catch = 0x0000,
        Filter = 0x0001,
        Finally = 0x0002,
        Fault = 0x0004,
    }
}