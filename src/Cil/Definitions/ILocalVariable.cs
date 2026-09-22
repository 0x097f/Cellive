namespace Cellive.Cil.Definitions
{
    public interface ILocalVariable
    {
        int Index { get; }
        string? Name { get; }
        ITypeDef? VariableType { get; }

        bool IsPinned { get; }
        bool IsByReference { get; }
        bool HasName { get; }
    }
}