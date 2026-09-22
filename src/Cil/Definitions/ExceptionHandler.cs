namespace Cellive.Cil.Definitions
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        public ExceptionHandlerType HandlerType { get; set; }

        public int TryStart { get; set; }
        public int TryEnd { get; set; }
        public int HandlerStart { get; set; }
        public int HandlerEnd { get; set; }
        public int FilterStart { get; set; } = -1;

        public ITypeDef? CatchType { get; set; }

        public bool HasFilter => FilterStart >= 0 && HandlerType == ExceptionHandlerType.Filter;

        public bool ContainsTryOffset(int offset)
            => offset >= TryStart && offset < TryEnd;

        public bool ContainsHandlerOffset(int offset)
            => offset >= HandlerStart && offset < HandlerEnd;

        public override string ToString()
        {
            return HandlerType switch
            {
                ExceptionHandlerType.Catch =>
                    $"catch ({CatchType?.Name ?? "?"}) [0x{TryStart:X4}-0x{TryEnd:X4}] -> [0x{HandlerStart:X4}-0x{HandlerEnd:X4}]",
                ExceptionHandlerType.Filter =>
                    $"filter [0x{FilterStart:X4}] -> [0x{HandlerStart:X4}-0x{HandlerEnd:X4}]",
                ExceptionHandlerType.Finally =>
                    $"finally [0x{TryStart:X4}-0x{TryEnd:X4}] -> [0x{HandlerStart:X4}-0x{HandlerEnd:X4}]",
                ExceptionHandlerType.Fault =>
                    $"fault [0x{TryStart:X4}-0x{TryEnd:X4}] -> [0x{HandlerStart:X4}-0x{HandlerEnd:X4}]",
                _ => "unknown"
            };
        }
    }
}
